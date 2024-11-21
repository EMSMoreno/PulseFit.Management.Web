// Função para alternar o modo escuro
const toggleDarkMode = () => {
    const elements = [
        document.body,
        document.querySelector('.sidebar'),
        document.querySelector('.topbar'),
        document.querySelector('.breadcrumb'),
        document.querySelector('.logout-btn')
    ];

    const cards = document.querySelectorAll('.card');

    // Alterna a classe 'dark-mode' para todos os elementos
    elements.forEach(el => el && el.classList.toggle('dark-mode'));
    cards.forEach(card => card.classList.toggle('dark-mode'));
};

// Função para animar os cards no dashboard
const animateCards = () => {
    document.querySelectorAll('.card').forEach(card => {
        card.classList.add('animate__animated', 'animate__fadeInUp');
    });
};

// Função para carregar e animar notificações
const loadNotifications = () => {
    const notificationIcon = document.querySelector('.notifications i');
    notificationIcon.classList.add('animate__animated', 'animate__shakeX');
    setTimeout(() => notificationIcon.classList.remove('animate__shakeX'), 1000);
};

// Função para mostrar notificações (placeholder)
const showNotifications = () => {
    const notifications = document.querySelector('.notifications');
    notifications.innerHTML = `<i class="fas fa-bell"></i><span class="badge">3</span>`;
    setTimeout(() => notifications.innerHTML = '', 3000); // Esconde notificações após 3 segundos
};

// Função para alternar a sidebar recolhida
const toggleSidebar = () => {
    const sidebar = document.querySelector('.sidebar');
    sidebar.classList.toggle('collapsed');
};

// Função para adicionar animação no botão de logout
const addLogoutAnimation = () => {
    const logoutButton = document.querySelector('.logout-btn');
    logoutButton.addEventListener('click', function () {
        this.classList.add('animate__animated', 'animate__bounce');
        setTimeout(() => this.classList.remove('animate__bounce'), 1000);
    });
};

// Função para gerenciar o comportamento dos submenus
const handleSubmenus = () => {
    // Manipula o clique nos links que têm submenus
    $('.sidebar-menu li > a[data-toggle="submenu"]').click(function (e) {
        e.preventDefault(); // Previne o comportamento padrão do link

        const submenu = $(this).next('.submenu'); // Seleciona o submenu correspondente
        $('.submenu').not(submenu).slideUp(); // Fecha outros submenus abertos
        submenu.slideToggle(300); // Abre ou fecha o submenu clicado
    });

    // Manipula o clique nos itens do submenu
    $('.submenu li > a').click(function () {
        // Não adicionamos lógica para fechar o submenu aqui
        // O link irá redirecionar para a nova view sem fechar o submenu
    });
};

// Inicialização das funções ao carregar a página
window.onload = () => {
    animateCards();
    showNotifications();
    loadNotifications();
    addLogoutAnimation();

    // Alternar modo escuro
    const darkModeToggle = document.querySelector('#darkModeToggle');
    if (darkModeToggle) {
        darkModeToggle.addEventListener('click', toggleDarkMode);
    }

    // Alternar sidebar
    const sidebarToggle = document.querySelector('#toggleSidebar');
    if (sidebarToggle) {
        sidebarToggle.addEventListener('click', toggleSidebar);
    }
};

// Inicialização dos submenus e outras funções ao carregar o DOM
$(document).ready(() => {
    handleSubmenus();
});
