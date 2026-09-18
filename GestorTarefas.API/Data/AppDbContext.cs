using Microsoft.EntityFrameworkCore;
using GestorTarefas.API.Models;

namespace GestorTarefas.API.Data
{
    // [AppDbContext : DbContext] Classe de contexto do banco de dados
    // Representa o contexto de acesso ao banco de dados da aplicação
    public class AppDbContext : DbContext // O AppDbContext funciona como uma espécie de ponte entre suas classes C# e o banco
    {
        // [AppDbContext(DbContextOptions<AppDbContext> options)] Construtor da classe AppDbContext
        // Recebe as opções de configuração do contexto, como a string de conexão com o banco
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // [DbSet<Tarefa>] Propriedades DbSet representam as tabelas do banco de dados
        // Estamos dizendo que esse contexto possui uma entidade chamada Tarefa, que será mapeada para uma tabela/conjunto de tarefas no banco de dados
        public DbSet<Tarefa> Tarefas { get; set; }

        // [DbSet<ItemChecklist>] Propriedade DbSet representam as tabelas do banco de dados
        // Estamos dizendo que esse contexto possui uma entidade chamada ItemChecklist, que será mapeada para uma tabela/conjunto de itens de checklist no banco de dados
        public DbSet<ItemChecklist> ItensChecklist { get; set; }
    }
}