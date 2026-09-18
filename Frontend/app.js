// URL base da API
const API_URL = 'http://localhost:5290/api/Tarefas';

// DOMContentLoaded é um evento disparado quando o documento HTML for carregado.
document.addEventListener('DOMContentLoaded', () => { // Quando o DOM estiver carregado, executa a função para carregar tarefas
    carregarTarefas();

    const form = document.getElementById('form-tarefa'); // Seleciona o formulário de cadastro de tarefas
    form.addEventListener('submit', cadastrarTarefa); // Adiciona um listener para o evento de submit do formulário, chamando a função cadastrarTarefa
});

// Função para buscar tarefas da API (GET)
async function carregarTarefas() { // Função assíncrona para carregar tarefas
    try {
        const response = await fetch(API_URL); // Chamada GET para a API
        const tarefas = await response.json(); // Parse do JSON retornado
        renderizarTarefas(tarefas); // Renderiza as tarefas no HTML
    } catch (error) {
        console.error('Erro ao buscar tarefas:', error); // Log de erro no console
    }
}

// Renderiza o HTML das tarefas
function renderizarTarefas(tarefas) { // Renderiza as tarefas no HTML
    const container = document.getElementById('lista-tarefas'); // Seleciona o container onde as tarefas serão exibidas
    container.innerHTML = ''; // Limpa o container antes de renderizar

    if (tarefas.length === 0) { // Se não houver tarefas, exibe uma mensagem
        container.innerHTML = '<p>Nenhuma tarefa cadastrada ainda.</p>';
        return;
    }

    tarefas.forEach(t => { // Itera sobre cada tarefa e cria um card para ela
        const prioridadeTexto = t.prioridade === 3 ? 'Alta' : t.prioridade === 2 ? 'Normal' : 'Baixa'; // Define o texto da prioridade com base no valor numérico
        const badgeClasse = t.prioridade === 3 ? 'badge-alta' : t.prioridade === 2 ? 'badge-normal' : 'badge-baixa'; // Define a classe do badge com base na prioridade
        // Cria o card da tarefa
        const card = document.createElement('div'); //  Cria um elemento div para o card da tarefa
        card.className = `tarefa-card prioridade-${t.prioridade}`; // Adiciona a classe de prioridade ao card para estilização
        // Define o conteúdo HTML do card, incluindo título, descrição, data de conclusão prevista e botão de exclusão
        card.innerHTML = `
            <div class="tarefa-info">
                <h3>${t.titulo} <span class="badge ${badgeClasse}">${prioridadeTexto}</span></h3>
                <p>${t.descricao || 'Sem descrição.'}</p>
                <small>Conclusão prevista: ${t.dataConclusaoPrevista ? new Date(t.dataConclusaoPrevista).toLocaleDateString() : 'Não informada'}</small>
            </div>
            <button class="btn-delete" onclick="deletarTarefa(${t.id})">Excluir</button>
        `;
        // Adiciona o card ao container
        container.appendChild(card);
    });
}

// Função para enviar nova tarefa para a API (POST)
async function cadastrarTarefa(event) {
    event.preventDefault(); // Previne o comportamento padrão do formulário (recarregar a página)

    const titulo = document.getElementById('titulo').value; // Pega o valor do campo de título
    const descricao = document.getElementById('descricao').value; // Pega o valor do campo de descrição
    const prioridade = parseInt(document.getElementById('prioridade').value); // Pega o valor do campo de prioridade
    const dataConclusao = document.getElementById('dataConclusao').value; // Pega o valor do campo de data de conclusão
    // Cria um objeto com os dados da nova tarefa
    const novaTarefa = {
        titulo: titulo,
        descricao: descricao,
        prioridade: prioridade,
        status: 1, // Pendente
        dataConclusaoPrevista: dataConclusao ? new Date(dataConclusao).toISOString() : null // Converte a data para o formato ISO, ou null se não informado
    };
    // Envia a nova tarefa para a API usando fetch com método POST
    try {
        const response = await fetch(API_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(novaTarefa)
        });
        // Verifica se a resposta da API foi bem-sucedida
        if (response.ok) {
            document.getElementById('form-tarefa').reset();
            carregarTarefas(); // Recarrega a lista
        } else {
            alert('Erro ao salvar tarefa.');
        }
    } catch (error) {
        console.error('Erro ao cadastrar:', error);
    }
}

// Função para excluir uma tarefa (DELETE)
async function deletarTarefa(id) {
    if (!confirm('Deseja realmente excluir esta tarefa?')) return; // Confirmação antes de excluir
    
    try {
        const response = await fetch(`${API_URL}/${id}`, { // Chamada DELETE para a API com o ID da tarefa
            method: 'DELETE'
        });
        // Verifica se a resposta da API foi bem-sucedida
        if (response.ok) {
            carregarTarefas();
        } else {
            alert('Erro ao excluir tarefa.');
        }
    } catch (error) {
        console.error('Erro ao deletar:', error);
    }
}