$(document).ready(function () {
    $('#pesquisar').click(function () {
        $.ajax({
            url: '/Cliente/Pesquisa',
            type: 'GET',
            data: $('#searchForm').serialize(),
            success: function (data) {
                $('#resultContainer').html(data);
            },
            error: function () {
                alert('Erro ao carregar dados');
            }
        });
    });

    $('#searchForm').on('reset', function () {
        $.ajax({
            url: '/Cliente/Consulta',
            type: 'GET',
            success: function (data) {
                $('#resultContainer').html($(data).find('#resultContainer').html());
            },
            error: function () {
                alert('Erro ao carregar dados');
            }
        });
    });
});

function openEditModal(button) {
    var clienteId = $(button).data('id');
    var nome = $(button).data('nome');
    var cpf = $(button).data('cpf');
    var dataNascimento = $(button).data('datanascimento');
    var endereco = $(button).data('endereco');
    var estado = $(button).data('estado');
    var cidade = $(button).data('cidade');
    var sexo = $(button).data('sexo');

    $('#editClienteId').val(clienteId);
    $('#editNome').val(nome);
    $('#editCpf').val(cpf);
    $('#editDataNascimento').val(dataNascimento);
    $('#editEndereco').val(endereco);
    $('#editEstado').val(estado);
    $('#editCidade').val(cidade);
    $('#editSexo').val(sexo);

    $('#editClienteModal').modal('show');
}

// Fetch estados from IBGE API
$.ajax({
    url: 'https://servicodados.ibge.gov.br/api/v1/localidades/estados',
    method: 'GET',
    success: function (data) {
        var estadoSelect = $('#Estado');
        data.sort(function (a, b) {
            return a.nome.localeCompare(b.nome);
        });
        data.forEach(function (estado) {
            estadoSelect.append(new Option(estado.nome, estado.sigla));
        });
    }
});

// Fetch cidades based on selected estado
$('#Estado').change(function () {
    var estadoSigla = $(this).val();
    if (estadoSigla) {
        $.ajax({
            url: 'https://servicodados.ibge.gov.br/api/v1/localidades/estados/' + estadoSigla + '/distritos',
            method: 'GET',
            success: function (data) {
                var cidadeSelect = $('#Cidade');
                cidadeSelect.empty();
                cidadeSelect.append(new Option('Selecione', ''));
                data.sort(function (a, b) {
                    return a.nome.localeCompare(b.nome);
                });
                data.forEach(function (cidade) {
                    cidadeSelect.append(new Option(cidade.nome, cidade.nome));
                });
            }
        });
    } else {
        $('#Cidade').empty();
        $('#Cidade').append(new Option('Selecione', ''));
    }
});

function saveEdicaoCliente() {
    var cliente = {
        Id: document.getElementById('editClienteId').value,
        Nome: document.getElementById('editNome').value,
        Cpf: document.getElementById('editCpf').value,
        DataNascimento: document.getElementById('editDataNascimento').value,
        Endereco: document.getElementById('editEndereco').value,
        Estado: document.getElementById('editEstado').value,
        Cidade: document.getElementById('editCidade').value,
        Sexo: document.getElementById('editSexo').value
    };

    console.log('Dados para salvar:', cliente);

    fetch('/Cliente/Editar', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
        },
        body: JSON.stringify(cliente)
    })
        .then(response => {
            console.log('Response status:', response.status);
            console.log('Response status text:', response.statusText);
            return response.text().then(text => {
                console.log('Response text:', text);
                try {
                    return text ? JSON.parse(text) : {};
                } catch (error) {
                    throw new Error('Erro ao analisar a resposta JSON: ' + error.message + ' - Resposta: ' + text);
                }
            });
        })
        .then(data => {
            console.log('Dados recebidos após salvar:', data);
            if (data.success) {
                $('#editClienteModal').modal('hide');
                location.reload();
            } else {
                alert('Erro ao salvar o cliente: ' + (data.message || 'Erro desconhecido'));
            }
        })
        .catch(error => {
            console.error('Erro ao salvar o cliente:', error);
            alert('Erro ao salvar o cliente: ' + error.message);
        });
}