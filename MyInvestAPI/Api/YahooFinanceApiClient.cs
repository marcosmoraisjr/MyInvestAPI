using Flurl.Util;
using MyInvestAPI.Domain;
using System.Runtime.CompilerServices;
using YahooFinanceApi;

namespace MyInvestAPI.Api;

public class YahooFinanceApiClient
{
    public async static Task<ActiveReturn> GetActive(string active)
    {
        var securitie = await Yahoo.Symbols(active).Fields(
                Field.DividendDate, //data
                Field.Symbol, //ativo
                Field.LongName, //nome do ativo
                Field.QuoteType, //tipo do ativo
                Field.TrailingAnnualDividendYield, //divident yield
                Field.RegularMarketPrice, //preço atual
                Field.PriceToBook, // P/VPs
                Field.TrailingPE  // P/L
                                  // Roe
            ).QueryAsync();

        var result = securitie[$"{active}"];

        return await CreateActiveReturn(result);
    }

    static async Task<ActiveReturn> CreateActiveReturn(Security result)
    {
        decimal tetoPrice = CalculatePriceTeto((decimal)result.TrailingAnnualDividendYield, (decimal)result.RegularMarketPrice);
        string recomendation = Recomendation((decimal)result.RegularMarketPrice, tetoPrice);

        DateTime currentDate = DateTime.Now;

        ActiveReturn activeReturn = new();
        activeReturn.Data = currentDate.ToString("dd-MM-yyyy");
        activeReturn.Ativo = result.Symbol;
        activeReturn.NomeDoAtivo = result.LongName;
        activeReturn.Tipo = VerifyType(result.QuoteType);
        activeReturn.DividentYield = (result.TrailingAnnualDividendYield * 100).ToString("F2") + "%";
        activeReturn.PrecoAtual = $"R$ {result.RegularMarketPrice.ToString("F2")}";
        activeReturn.P_VP = (result.PriceToBook).ToString("F1");
        activeReturn.Preco_Teto = $"R$ {tetoPrice.ToString("F2")}";
        activeReturn.Indicacao = recomendation;
        activeReturn.P_L = (result.TrailingPE).ToString("F1");
        activeReturn.Crecimento_De_Dividendos_5_anos = await CalculateDividendGrowth(result.Symbol);

        return activeReturn;
    }

    //static async Task<string> CalculateRoe(string symbol)
    //{
    //    var stock = await Yahoo.GetStockAsync(ticker);
    //}

    static async Task<string> CalculateDividendGrowth(string symbol)
    {
        DateTime startDate = DateTime.Now.AddYears(-5);
        DateTime endDate = DateTime.Now;

        var history = await Yahoo.GetDividendsAsync(symbol, startDate, endDate);


        var dividends = history.Where(x => x.Dividend != null && x.Dividend > 0)
                               .Select(c => new
                               {
                                   c.DateTime,
                                   c.Dividend
                               })
                               .ToList();

        if (dividends.Count == 0)
        {
            return "Dados indisponíveis";
        }

        var dividendsPerYear = dividends.GroupBy(d => d.DateTime.Year)
                                        .Select(g => new
                                        {
                                            Year = g.Key,
                                            DividendsTotal = g.Sum(d => d.Dividend)
                                        })
                                        .ToList();

        double averageDividends = (double)dividendsPerYear.Average(d => d.DividendsTotal);

        return $"{(averageDividends * 100).ToString("0.##") + "%"} por ano.";
    }

    static decimal CalculatePriceTeto(decimal dividendYield, decimal RegularMarketPrice)
    {
        decimal desiredReturnRate = 0.06M;

        //calculates the average dividend per share
        decimal dividendPerShare = RegularMarketPrice * dividendYield;

        return dividendPerShare / desiredReturnRate;
    }

    static string Recomendation(decimal regularMarketPrice, decimal tetoPrice)
    {
        return regularMarketPrice <= tetoPrice ? "🟢 Comprar" : "🔴 Não-comprar";
    }

    static string VerifyType(string type)
    {
        if (type == "EQUITY")
        {
            return "Ação";
        }
        else if (type == "ETF" || type == "REIT")
        {
            return "FII";
        }
        return "Tipo não expecificado";
    }
}
