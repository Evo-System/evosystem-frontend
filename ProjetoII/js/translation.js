// Função para carregar as traduções de um arquivo JSON
async function fetchTranslations(lang) {
    try {
        const response = await fetch(`locales/${lang}.json`);
        if (!response.ok) {
            throw new Error(`Could not load ${lang}.json`);
        }
        return await response.json();
    } catch (error) {
        console.error(error);
        // Tenta carregar o idioma padrão (inglês) em caso de falha
        const response = await fetch('locales/en.json');
        return await response.json();
    }
}

// Função para aplicar as traduções na página
function applyTranslations(translations) {
    document.querySelectorAll('[data-i18n]').forEach(element => {
        const key = element.dataset.i18n;
        if (translations[key]) {
            element.innerText = translations[key];
        }
    });
}

// Função para definir o idioma e salvar no localStorage
async function setLanguage(lang) {
    localStorage.setItem('language', lang);
    const translations = await fetchTranslations(lang);
    applyTranslations(translations);
}

// Função para carregar o idioma salvo ou o padrão do navegador
async function initLocalization() {
    // 1. Obtém o idioma salvo
    let savedLang = localStorage.getItem('language');

    // 2. Se não houver idioma salvo, usa o idioma do navegador (com 'pt' como fallback)
    if (!savedLang) {
        savedLang = navigator.language.startsWith('pt') ? 'pt' : 'en';
    }

    // 3. Aplica o idioma
    await setLanguage(savedLang);

    // 4. Atualiza o <select> na página de opções (se existir)
    const languageSelect = document.getElementById('language-select');
    if (languageSelect) {
        languageSelect.value = savedLang;
    }
}

// Evento principal que dispara quando a página é carregada
document.addEventListener('DOMContentLoaded', () => {
    initLocalization();

    // Encontra o botão de salvar APENAS na página de opções
    const saveButton = document.getElementById('save-settings-btn');
    if (saveButton) {
        saveButton.addEventListener('click', (e) => {
            e.preventDefault(); // Impede o envio de formulário
            
            const languageSelect = document.getElementById('language-select');
            const selectedLang = languageSelect.value;
            
            setLanguage(selectedLang);
            
            // Opcional: Mostrar uma confirmação de "Salvo!"
            const originalText = saveButton.innerText;
            saveButton.innerText = 'Salvo!';
            setTimeout(() => {
                saveButton.innerText = originalText;
                // Recarrega as traduções para o botão "Salvar"
                initLocalization(); 
            }, 1500);
        });
    }
});