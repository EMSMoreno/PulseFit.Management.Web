// Função para pré-visualização da imagem ao criar um aluno
function setupImageUploadPreview(inputId, previewId) {
    document.getElementById(inputId).addEventListener("change", function (event) {
        const reader = new FileReader();
        reader.onload = function () {
            const preview = document.getElementById(previewId);
            preview.src = reader.result; // Atualiza a pré-visualização com a nova imagem
        };
        reader.readAsDataURL(event.target.files[0]);
    });
}

// Função para pré-visualização da nova imagem ao editar um aluno
function setupNewImagePreview(inputId, newPreviewId, currentImageId) {
    document.getElementById(inputId).addEventListener("change", function (event) {
        const file = event.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                // Mostra a nova imagem e oculta a antiga
                document.getElementById(newPreviewId).src = e.target.result;
                document.getElementById(newPreviewId).style.display = "block"; // Exibe a nova pré-visualização
                document.getElementById(currentImageId).style.display = "none"; // Oculta a imagem atual
            };
            reader.readAsDataURL(file);
        }
    });
}

// Função para configurar o SweetAlert ao submeter o formulário
function setupSweetAlertOnSubmit(formId, successMessage) {
    const form = document.getElementById(formId);

    form.addEventListener("submit", function (event) {
        event.preventDefault(); // Previne o envio padrão

        // Verifique se o formulário passou na validação do lado do cliente
        if (form.checkValidity()) {
            Swal.fire({
                title: 'Are you sure?',
                text: successMessage,
                icon: 'success',
                showCancelButton: true,
                confirmButtonText: 'Yes, submit it!',
            }).then((result) => {
                if (result.isConfirmed) {
                    form.submit(); // Apenas submete o formulário se confirmado
                }
            });
        } else {
            // Se a validação falhar, exiba uma mensagem de erro
            Swal.fire({
                title: 'Validation Error',
                text: 'Please correct the errors in the form.',
                icon: 'error',
            });
        }
    });
}


// Função para configurar o SweetAlert para exclusão
function setupSweetAlertOnDelete(buttonId, formId, confirmMessage) {
    document.getElementById(buttonId).addEventListener("click", function () {
        Swal.fire({
            title: 'Are you sure?',
            text: confirmMessage,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                document.getElementById(formId).submit(); // Submete o formulário se o usuário confirmar
            }
        });
    });
}

