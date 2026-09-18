namespace GestorTarefas.API.Models
{
    public enum PrioridadeEnum
    {
        Baixa = 1,
        Normal = 2,
        Alta = 3
    }

    public enum StatusEnum
    {
        Pendente = 1,
        EmAndamento = 2,
        Concluida = 3
    }

    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public PrioridadeEnum Prioridade { get; set; } = PrioridadeEnum.Normal;
        public StatusEnum Status { get; set; } = StatusEnum.Pendente;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataConclusaoPrevista { get; set; }
        
        // Relacionamento com itens de checklist
        public List<ItemChecklist> Checklist { get; set; } = new();
    }

    public class ItemChecklist
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public bool Concluido { get; set; } = false;

        // Chave estrangeira para vincular à tarefa
        public int TarefaId { get; set; }
    }
}