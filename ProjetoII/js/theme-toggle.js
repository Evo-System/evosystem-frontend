document.addEventListener('DOMContentLoaded', () => {
    const themeToggleBtn = document.getElementById('theme-toggle');
    const body = document.body;

    // Função para definir o tema (e ícone)
    const setTheme = (theme) => {
        body.classList.remove('theme-dark', 'theme-light');
        body.classList.add(theme);
        localStorage.setItem('theme', theme);
    };

    // Carregar tema salvo ou definir padrão (escuro)
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme) {
        setTheme(savedTheme);
    } else {
        setTheme('theme-dark'); // Tema padrão
    }

    // Adicionar o evento de clique ao botão
    themeToggleBtn.addEventListener('click', () => {
        if (body.classList.contains('theme-dark')) {
            setTheme('theme-light');
        } else {
            setTheme('theme-dark');
        }
    });
});