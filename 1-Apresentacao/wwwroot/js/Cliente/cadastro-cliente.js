$(document).ready(function () {

    $('#Cpf').mask('000.000.000-00', { reverse: true });

    function setTodayDate() {
        var today = new Date().toISOString().split('T')[0];
        $('#DataNascimento').val(today);
    }

    var dataNascimento = $('#DataNascimento').val();
    if (!dataNascimento || dataNascimento === "0001-01-01") {
        setTodayDate();
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

    $('#resetBtn').click(function () {
        setTimeout(setTodayDate, 0);
    });

    $("#clienteForm").validate({
        errorClass: "is-invalid",
        validClass: "is-valid",
        errorPlacement: function (error, element) {
            if (element.attr("name") == "Sexo") {
                error.insertAfter(element.closest(".form-check-inline").last());
            } else {
                error.insertAfter(element);
            }
        },
        rules: {
            Cpf: {
                required: true,
                cpfBR: true
            },
            Nome: "required",
            DataNascimento: "required",
            Sexo: "required",
            Endereco: "required",
            Estado: "required",
            Cidade: "required"
        },
        messages: {
            Cpf: {
                required: "Por favor, insira o CPF",
                cpfBR: "Por favor, insira um CPF válido"
            },
            Nome: "Por favor, insira o nome",
            DataNascimento: "Por favor, insira a data de nascimento",
            Sexo: "Por favor, selecione o sexo",
            Endereco: "Por favor, insira o endereço",
            Estado: "Por favor, selecione o estado",
            Cidade: "Por favor, selecione a cidade"
        }
    });

    $.validator.addMethod("cpfBR", function (value, element) {
        value = value.replace(/[^\d]+/g, '');
        if (value.length !== 11) {
            return false;
        }
        var sum = 0,
            remainder;
        for (var i = 1; i <= 9; i++) {
            sum = sum + parseInt(value.substring(i - 1, i)) * (11 - i);
        }
        remainder = (sum * 10) % 11;
        if ((remainder === 10) || (remainder === 11)) {
            remainder = 0;
        }
        if (remainder !== parseInt(value.substring(9, 10))) {
            return false;
        }
        sum = 0;
        for (i = 1; i <= 10; i++) {
            sum = sum + parseInt(value.substring(i - 1, i)) * (12 - i);
        }
        remainder = (sum * 10) % 11;
        if ((remainder === 10) || (remainder === 11)) {
            remainder = 0;
        }
        return remainder === parseInt(value.substring(10, 11));
    }, "Por favor, insira um CPF válido.");
});
