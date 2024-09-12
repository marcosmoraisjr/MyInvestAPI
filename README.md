# MyInvest
Uma aplicação web (C# | ASP.NET + Angular) com o objetivo de fornecer informações sobre carteira de ativos de usuários cadastrados, como o preço-teto (modelo de Bazin) e, se estiver abaixo do preço atual, sugerir ou não a compra do ativo.

## ⚙️Como rodar o projeto:
1. Tenha o Docker instalado e rodando na sua máquina.
2. Clone o projeto na sua máquina:
```
https://github.com/gabrielcruzrodrigues/MyInvestAPI.git
```
3. Acesse a pasta do projeto:
```
cd MyInvest/
```
4. Execute o comando para acionar o Docker-compose:
```
docker-compose up --build
```

## 🟢Como acessar o projeto:
### 🖥️Interface
A interface do programa pode ser acessada logo após a inicialização do projeto, usando a url abaixo:
```
http://localhost:9090
```

### 📓Documentação da API
A documentação está disponível utilizando o Swagger, você pode acessá-la e ver todos os endpoints e seus detalhes acessando:
```
http://localhost:8080/swagger/index.html
```
