import Vue from "vue";
import { getHeaders, getValidationPath } from "../helpers";
import axios from "axios";

const qs = require("qs");

const ValidateMultiple = Vue.extend({
    props: {
        model: String,
        properties: Array,
        isDateValidation: Boolean
    },
    data: function () {
        return {
            hasError: false,
            errorFields: []
        }
    },
    methods: {
        // Annotate values to compare with ref="input[i]", and error fields with ref="errorField[i]", for any number of inputs that should be validated together
        // Each error message will be mapped to the corresponding field.
        validate: function () {
            const inputs = this.createArrayFromRefElements("input");
            var queryString: string;
            if (this.isDateValidation) {
                // In the case of data validation, inputs will be vue components (ValidateDate.ts)
                const vueInputs: Array<Vue> = inputs;
                var dayInputs = vueInputs.map(i => (<HTMLInputElement> i.$refs.dayInput).value);
                if (arrayContainsEmptyValues(dayInputs)) {
                    return;
                }
                var monthInputs = vueInputs.map(i => (<HTMLInputElement> i.$refs.monthInput).value);
                if (arrayContainsEmptyValues(monthInputs)) {
                    return;
                }
                var yearInputs = vueInputs.map(i => (<HTMLInputElement> i.$refs.yearInput).value);
                if (arrayContainsEmptyValues(yearInputs)) {
                    return;
                }
                queryString = buildKeyDateValuesQueryString(this.properties, dayInputs, monthInputs, yearInputs);
                this.errorFields = inputs.map((i: any) => i.$refs.errorField);
            }
            else if (inputs[0].type === "radio") {
                // TODO: Do this mapping for other types if element is reused for non-radio inputs.
                const radioInputs: Array<HTMLInputElement> = inputs;
                var inputValues = radioInputs.map(i => i.checked);
                queryString = buildKeyValuePairsQueryString(this.properties, inputValues);
                this.errorFields = this.createArrayFromRefElements("errorField");
            }

            axios.get(`${getValidationPath(this.$props.model)}${this.isDateValidation ? 'Dates' : 'Properties'}?${queryString}`, { headers: getHeaders() })
                .then((response: any) => {
                    var errorMessages = response.data;
                    this.hasError = !!errorMessages;

                    if (this.hasError) {
                        this.errorFields.forEach((errorField: any, index: number) => {
                            errorField.textContent = errorMessages[index];
                            errorField.classList.remove("hidden");
                        });
                    } else {
                        this.errorFields.forEach((errorField: any, index: number) => {
                            errorField.textContent = "";
                            errorField.classList.add("hidden");
                        });
                    }
                })
                .catch((error: any) => {
                    console.log(error.response);
                });
        },
        createArrayFromRefElements: function (elementName: string) {
            const array = [];
            for (let i = 0; i < this.properties.length; i++) {
                array.push(this.$refs[`${elementName}[${i}]`]);
            }
            return array;
        }
    }
});

type KeyValuePairs = Array<{[key: string]: string}>

function buildKeyValuePairsQueryString(keys: Array<string>, values: Array<boolean>): string {
    const keyValuePairs: KeyValuePairs = [];
    for (let i = 0; i < keys.length; i++) {
        keyValuePairs.push({ key: keys[i], value: values[i].toString() });
    }
    return qs.stringify({keyValuePairs});
};

function buildKeyDateValuesQueryString(keys: Array<string>, days: Array<string>, months: Array<string>, years: Array<string>): string {
    const keyValuePairs: KeyValuePairs = [];
    for (let i = 0; i < keys.length; i++) {
        keyValuePairs.push({ key: keys[i], day: days[i], month: months[i], year: years[i] });
    }
    return qs.stringify({keyValuePairs});
};

function arrayContainsEmptyValues(array: Array<string>): boolean {
    return array.some(el => !el);
}

export {
    ValidateMultiple
};