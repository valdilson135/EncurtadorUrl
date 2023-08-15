<h1 align="center"> Encurtador de Urls </h1>

<p align="center">
<img loading="lazy" src="http://img.shields.io/static/v1?label=STATUS&message=EM%20DESENVOLVIMENTO&color=GREEN&style=for-the-badge"/>
</p>

# Aplicação de Encurtamento de URLs (Web API)

Bem-vindo à documentação da aplicação de encurtamento de URLs. Esta aplicação é uma Web API que permite que os usuários enviem URLs longas e recebam uma versão encurtada para compartilhar facilmente.

## Funcionalidades Principais

- Encurtamento de URLs: Os usuários podem enviar URLs longas e receber uma versão encurtada gerada aleatoriamente.
- Consultas: Quando um usuário consultar uma URL encurtada, é retornado a URL original correspondente.
- Estatísticas: Os usuários podem acompanhar as estatísticas básicas das URLs encurtadas, como o número de consultas.

## Tecnologias Utilizadas

- IDE: Visual Studio 2022 Community Edition
- Linguagem: C#
- Framework: .NET Core 6
- Banco de Dados: PostgreSQL
- Docker: Utilizado para empacotar a aplicação e o banco de dados em contêineres isolados.
- RabbitMq: Utilizado para gravar as url em uma fila.
  
## Configuração

1. **Clonagem do Repositório:** Clone este repositório para a sua máquina local.
   
2. **Banco de Dados PostgreSQL:**
   - Certifique-se de ter o Docker instalado. 
   - Atualize as configurações de conexão com o banco de dados no arquivo `appsettings.json`.
   - Atualize as configurações de docker no arquivos `docker-compose, DockerFile`.
3. **Executando a Aplicação:**
   - Abra o projeto no Visual Studio 2022.
   - Configure as strings de conexão no arquivo `appsettings.json`.
   - Abrir o cmd e executar o comando a seguir `docker network create mottu`.
   - Execute a aplicação.

## Uso

1. Acesse a API via cliente HTTP ou ferramenta como [Postman](https://www.postman.com/).
2. Envie uma solicitação POST para `/api/Url` com a URL longa no corpo da requisição.
3. Receba a URL encurtada gerada como resposta.

## Endpoints da API

- `POST /api/Url`: Encurta uma URL. (Corpo: `{ "url": "URL_LONGA_AQUI" }`)

- Mais detalhes sobre outros endpoints podem ser encontrados na documentação da API.
