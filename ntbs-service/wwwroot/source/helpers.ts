var getHeaders = function() {
    return {
        "RequestVerificationToken": (<HTMLInputElement>document.querySelector('[name="__RequestVerificationToken"]')).value
    }
}

var getValidationPath = function(modelName: string) {
    return `${window.location.pathname}/Validate${modelName}`;
}

type FormattedDate = { day: any, month: any, year: any };

var convertFormattedDateToDate = function(date: FormattedDate) {
    return new Date(date.year, date.month - 1, date.day)
}

export { 
    getHeaders, getValidationPath, FormattedDate, convertFormattedDateToDate
};