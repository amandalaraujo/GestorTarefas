using Microsoft.EntityFrameworkCore; // Importa o namespace do Entity Framework Core, que é a biblioteca usada para interagir com bancos de dados relacionais no .NET 
using GestorTarefas.API.Data; // Importa o namespace do projeto GestorTarefas.API.Data, que contém a classe AppDbContext, responsável por gerenciar o acesso ao banco de dados

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados usando Entity Framework Core com SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))); // Configura o contexto do banco de dados (AppDbContext) para usar SQLite como provedor de banco de dados,

// Adiciona os serviços para Controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(); // Adiciona o middleware do Swagger UI, que fornece uma interface web interativa para explorar e testar a API. Isso é útil durante o desenvolvimento para verificar os endpoints da API e enviar solicitações de teste diretamente do navegador.
    app.UseSwagger(); // Adiciona o middleware do Swagger, que gera a documentação da API em formato JSON. Essa documentação é usada pelo Swagger UI para exibir os endpoints da API e suas informações, como parâmetros, respostas e descrições.
}

app.UseCors("PermitirTudo"); // Configura o middleware de CORS para permitir solicitações de qualquer origem, método e cabeçalho, facilitando o desenvolvimento e testes da API a partir de diferentes domínios.

app.UseHttpsRedirection(); // Adiciona o middleware de redirecionamento HTTPS, garantindo que todas as solicitações HTTP sejam redirecionadas para HTTPS, aumentando a segurança da aplicação.

app.UseAuthorization(); // Adiciona o middleware de autorização, que verifica se o usuário tem permissão para acessar determinados recursos da API. No entanto, como não há autenticação configurada, este middleware não terá efeito prático neste momento.

app.MapControllers(); // Mapeia os controladores da API para rotas, permitindo que as solicitações HTTP sejam direcionadas para os métodos apropriados nos controladores.

app.Run();