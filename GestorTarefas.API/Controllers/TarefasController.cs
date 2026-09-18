using Microsoft.AspNetCore.Mvc; // Necessário para usar ControllerBase e ActionResult
using Microsoft.EntityFrameworkCore; // Necessário para usar Include e ToListAsync
using GestorTarefas.API.Data; // Necessário para usar AppDbContext
using GestorTarefas.API.Models; // Necessário para usar Tarefa e ItemChecklist

namespace GestorTarefas.API.Controllers
{
    [ApiController] // Indica que esta classe é um controlador de API, permitindo que ela responda a solicitações HTTP e retorne dados no formato JSON
    [Route("api/[controller]")] // Define a rota base para este controlador. O [controller] será substituído pelo nome do controlador, neste caso, "Tarefas", resultando na rota "api/Tarefas"
    public class TarefasController : ControllerBase
    {
        // Declaração de uma variável privada do tipo AppDbContext, que será usada para interagir com o banco de dados
        private readonly AppDbContext _context;

        // Construtor do controlador que recebe uma instância de AppDbContext via injeção de dependência. 
        // Isso permite que o controlador acesse o banco de dados sem precisar criar uma nova instância do contexto manualmente
        public TarefasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Tarefas (Listar todas as tarefas com seus checklists)
        [HttpGet]
        // Método assíncrono que retorna uma lista de todas as tarefas, incluindo seus itens de checklist associados.
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetTarefas()
        {
            return await _context.Tarefas
                .Include(t => t.Checklist)
                .ToListAsync();
        }

        // GET: api/Tarefas/5 (Buscar uma tarefa por ID)
        [HttpGet("{id}")]
        // Método assíncrono que retorna uma tarefa específica pelo seu ID, incluindo seus itens de checklist associados.
        public async Task<ActionResult<Tarefa>> GetTarefa(int id)
        {
            var tarefa = await _context.Tarefas
                .Include(t => t.Checklist)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tarefa == null)
            {
                return NotFound(new { mensagem = "Tarefa não encontrada." });
            }

            return tarefa;
        }

        // POST: api/Tarefas (Criar uma nova tarefa)
        [HttpPost]
        // Método assíncrono que cria uma nova tarefa no banco de dados. Recebe um objeto Tarefa no corpo da solicitação e o adiciona ao contexto do banco de dados.
        public async Task<ActionResult<Tarefa>> PostTarefa(Tarefa tarefa)
        {
            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTarefa), new { id = tarefa.Id }, tarefa);
        }

        // PUT: api/Tarefas/5 (Atualizar uma tarefa existente)
        [HttpPut("{id}")]
        // Método assíncrono que atualiza uma tarefa existente no banco de dados. Recebe o ID da tarefa na URL e um objeto Tarefa no corpo da solicitação. Se o ID da URL não corresponder ao ID do objeto, retorna um erro de requisição inválida. 
        // Caso contrário, atualiza a tarefa e salva as alterações no banco de dados.
        public async Task<IActionResult> PutTarefa(int id, Tarefa tarefa)
        {
            if (id != tarefa.Id)
            {
                return BadRequest(new { mensagem = "ID da URL diverge do ID do objeto enviado." });
            }

            _context.Entry(tarefa).State = EntityState.Modified;
            
            // O bloco try-catch é usado para capturar exceções de concorrência de atualização do banco de dados. Se a tarefa não existir mais (ou seja, foi excluída por outro processo), retorna um erro 404 Not Found. 
            // Caso contrário, a exceção é lançada novamente para ser tratada em outro lugar.
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TarefaExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Tarefas/5 (Excluir uma tarefa)
        [HttpDelete("{id}")]
        // Método assíncrono que exclui uma tarefa existente no banco de dados. Recebe o ID da tarefa na URL, busca a tarefa correspondente no banco de dados e, se encontrada, remove-a do contexto e salva as alterações.
        public async Task<IActionResult> DeleteTarefa(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null)
            {
                return NotFound();
            }

            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Tarefas/5/checklist (Adicionar um item de checklist a uma tarefa)
        [HttpPost("{tarefaId}/checklist")]
        // Método assíncrono que adiciona um item de checklist a uma tarefa existente. Recebe o ID da tarefa na URL e um objeto ItemChecklist no corpo da solicitação.
        public async Task<ActionResult<ItemChecklist>> AddItemChecklist(int tarefaId, ItemChecklist item)
        {
            var tarefa = await _context.Tarefas.FindAsync(tarefaId);
            if (tarefa == null)
            {
                return NotFound(new { mensagem = "Tarefa não encontrada." });
            }

            item.TarefaId = tarefaId;
            _context.ItensChecklist.Add(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        // PUT: api/Tarefas/checklist/5/status (Alternar status do item de checklist)
        [HttpPut("checklist/{itemId}/status")]
        // Método assíncrono que alterna o status de conclusão de um item de checklist. Recebe o ID do item na URL e um valor booleano no corpo da solicitação indicando se o item está concluído ou não.
        public async Task<IActionResult> ToggleItemStatus(int itemId, [FromBody] bool concluido)
        {
            var item = await _context.ItensChecklist.FindAsync(itemId);
            if (item == null)
            {
                return NotFound(new { mensagem = "Item de checklist não encontrado." });
            }

            item.Concluido = concluido;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // Método privado que verifica se uma tarefa existe no banco de dados com base no seu ID. Retorna true se a tarefa existir e false caso contrário.
        private bool TarefaExists(int id)
        {
            return _context.Tarefas.Any(e => e.Id == id);
        }
    }
}