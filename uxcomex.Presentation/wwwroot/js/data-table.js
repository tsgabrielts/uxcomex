class DataTableManager {
    constructor(options) {
        this.searchInputId = options.searchInputId || 'searchInput';
        this.tableId = options.tableId;
        this.entityName = options.entityName;
        this.deleteHandler = options.deleteHandler;
        this.init();
    }

    init() {
        this.setupSearch();
        this.setupDeleteHandlers();
    }

    setupSearch() {
        const searchInput = document.getElementById(this.searchInputId);
        const table = document.getElementById(this.tableId);

        if (!searchInput || !table) return;

        const rows = table.getElementsByTagName('tbody')[0].getElementsByTagName('tr');

        searchInput.addEventListener('input', (e) => {
            this.filterTable(e.target.value, rows, table);
        });
    }

    filterTable(searchTerm, rows, table) {
        searchTerm = searchTerm.toLowerCase().trim();
        let visibleCount = 0;

        for (let i = 0; i < rows.length; i++) {
            const row = rows[i];
            const cells = row.getElementsByTagName('td');
            let found = false;

            for (let j = 0; j < cells.length - 1; j++) {
                const cellText = cells[j].textContent.toLowerCase();
                if (cellText.includes(searchTerm)) {
                    found = true;
                    break;
                }
            }

            if (found || searchTerm === '') {
                row.style.display = '';
                visibleCount++;
            } else {
                row.style.display = 'none';
            }
        }

        this.updateNoResultsMessage(searchTerm, visibleCount, table);
    }

    updateNoResultsMessage(searchTerm, visibleCount, table) {
        const existingMessage = document.getElementById('noResultsMessage');
        if (existingMessage) {
            existingMessage.remove();
        }

        if (visibleCount === 0 && searchTerm !== '') {
            const noResultsRow = document.createElement('tr');
            noResultsRow.id = 'noResultsMessage';
            const colspan = table.querySelector('thead tr').children.length;
            noResultsRow.innerHTML = `<td colspan="${colspan}" class="text-center text-muted py-3">
                Nenhum ${this.entityName.toLowerCase()} encontrado para "${searchTerm}"
            </td>`;
            table.getElementsByTagName('tbody')[0].appendChild(noResultsRow);
        }
    }

    setupDeleteHandlers() {
        const table = document.getElementById(this.tableId);
        if (!table) return;

        table.addEventListener('click', (e) => {
            if (e.target.classList.contains('delete-btn') || e.target.closest('.delete-btn')) {
                const button = e.target.classList.contains('delete-btn') ? e.target : e.target.closest('.delete-btn');
                const id = button.getAttribute('data-id');
                this.handleDelete(id, button);
            }
        });
    }

    async handleDelete(id, button) {
        const entityName = this.entityName;

        const result = await Swal.fire({
            title: 'Tem certeza?',
            text: `Deseja excluir este ${entityName.toLowerCase()}? Esta ação não pode ser desfeita.`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#dc3545',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Sim, excluir!',
            cancelButtonText: 'Cancelar',
            reverseButtons: true
        });

        if (result.isConfirmed) {
            this.performDelete(id, button);
        }
    }

    performDelete(id, button) {
        const originalText = button.innerHTML;
        button.innerHTML = '<i class="fa fa-spinner fa-spin"></i>';
        button.disabled = true;

        if (typeof $ === 'undefined') {
            Swal.fire({
                icon: 'error',
                title: 'Erro!',
                text: 'Erro interno: jQuery não carregado'
            });
            button.innerHTML = originalText;
            button.disabled = false;
            return;
        }

        const token = $('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            url: this.deleteHandler,
            type: "POST",
            data: {
                id: id,
                __RequestVerificationToken: token
            },
            success: (response) => {
                Swal.fire({
                    icon: 'success',
                    title: 'Sucesso!',
                    text: `${this.entityName} excluído com sucesso!`,
                    timer: 3000,
                    showConfirmButton: false
                });

                $('tr[data-id="' + id + '"]').fadeOut(500, function () {
                    $(this).remove();
                });
            },
            error: (xhr) => {
                console.log("Erro:", xhr.status, xhr.responseText);

                let errorMessage = 'Erro ao excluir o item. Tente novamente.';
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    errorMessage = xhr.responseJSON.message;
                }

                Swal.fire({
                    icon: 'error',
                    title: 'Erro!',
                    text: errorMessage
                });

                button.innerHTML = originalText;
                button.disabled = false;
            }
        });
    }
}

document.addEventListener('DOMContentLoaded', function () {
    if (document.getElementById('clientesTable')) {
        new DataTableManager({
            tableId: 'clientesTable',
            entityName: 'Cliente',
            deleteHandler: '/Clientes?handler=Delete'
        });
    }

    if (document.getElementById('produtosTable')) {
        new DataTableManager({
            tableId: 'produtosTable',
            entityName: 'Produto',
            deleteHandler: '/Produtos?handler=Delete'
        });
    }

    if (document.getElementById('pedidosTable')) {
        new DataTableManager({
            tableId: 'pedidosTable',
            entityName: 'Pedido',
            deleteHandler: '/Pedidos?handler=Delete'
        });
    }
});