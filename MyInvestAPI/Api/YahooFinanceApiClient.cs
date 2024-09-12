using MyInvestAPI.Domain;
using YahooFinanceApi;
using MyInvestAPI.Extensions;

namespace MyInvestAPI.Api;

public class YahooFinanceApiClient
{
    public async static Task<ActiveReturn> GetActive(string active, string dYDesiredPercentage)
    {
        var search = await Yahoo.Symbols(active).Fields(
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

        if (search is null)
            throw new KeyNotFoundException();

        var result = search[$"{active}"];

        return await CreateActiveReturn(result, dYDesiredPercentage);
    }

    static async Task<ActiveReturn> CreateActiveReturn(Security result, string dYDesiredPercentage)
    {
        if (!decimal.TryParse(dYDesiredPercentage, out decimal dyDesired))
            throw new ArgumentException("Porcentagem inválida.");

        decimal dYCurrent = result[Field.TrailingAnnualDividendYield] != null ? Convert.ToDecimal(result[Field.TrailingAnnualDividendYield]) : 0;
        decimal currentPrice = result[Field.RegularMarketPrice] != null ? Convert.ToDecimal(result[Field.RegularMarketPrice]) : 0;

        decimal tetoPrice = CalculatePriceTeto(dYCurrent, currentPrice, dyDesired);
        string recomendation = Recomendation((decimal)result.RegularMarketPrice, tetoPrice);

        DateTime currentDate = DateTime.Now;

        ActiveReturn activeReturn = new();
        activeReturn.Data = currentDate.ToString("dd-MM-yyyy");
        activeReturn.Ativo = result.Symbol;
        activeReturn.NomeDoAtivo = result.LongName;
        activeReturn.Tipo = VerifyType(result.QuoteType);
        activeReturn.DividentYield = (dyDesired).ToString() + "%";
        activeReturn.PrecoAtual = $"R$ {result.RegularMarketPrice.ToString("F2")}";
        activeReturn.P_VP = (result.PriceToBook).ToString("F1");
        activeReturn.Preco_Teto = $"R$ {tetoPrice.ToString("F2")}";
        activeReturn.Indicacao = recomendation;
        activeReturn.P_L = (result.TrailingPE).ToString("F1");
        activeReturn.ROE = "Atualmente indisponível no nosso sistema";
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
        IEnumerable<DividendTick> history;

        try
        {
            history = await Yahoo.GetDividendsAsync(symbol, startDate, endDate);
        }
        catch (Exception)
        {
            return "Serviço indisponível";
        }

        if (history == null || !history.Any())
        {
            return "Dados indisponíveis";
        }

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

    static decimal CalculatePriceTeto(decimal dYCurrent, decimal currentPrice, decimal dYDesiredPercentage)
    {
        dYDesiredPercentage /= 100;

        if (dYCurrent <= 0 || currentPrice <= 0)
            throw new InvalidOperationException("Data of Dividend Yield or Active price are invalid!");

        decimal AnnualDividends = dYCurrent * currentPrice;

        decimal priceTeto = AnnualDividends / dYDesiredPercentage;

        return priceTeto;
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
