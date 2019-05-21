$('input[name="CategoriesStr"]').amsifySuggestags({
    type: 'amsify',
    suggestions: categoriesSuggestions
});
$('form input').on('keypress', function (e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13) {
        e.preventDefault();
        return false;
    }
});