var pluraliseModelName = function (name: string) {
    if (name.endsWith('s')) {
        return name;
    } else {
        return `${name}s`;
    }
}

var getValidationBaseBath = function(modelName: string) {
    let modelNamePlural = pluraliseModelName(modelName);
    return `/${modelNamePlural}/Edit/Validate${modelName}`;
}

type FormattedDate = { day: any, month: any, year: any };

var convertFormattedDateToDate = function(date: FormattedDate) {
    return new Date(date.year, date.month - 1, date.day)
}

export { 
    getValidationBaseBath, FormattedDate, convertFormattedDateToDate
};