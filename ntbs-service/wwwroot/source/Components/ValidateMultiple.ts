import Vue from "vue";
import { getHeaders, getValidationPath } from "../helpers";
import axios, {Method} from "axios";

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
            let requestData: KeyValuePairs;
            if (this.isDateValidation) {
                // In the case of data validation, inputs will be vue components (ValidateDate.ts)
                const vueInputs: Array<Vue> = inputs;
                const dayInputs = vueInputs.map(i => (<HTMLInputElement>i.$refs.dayInput).value);
                if (arrayContainsEmptyValues(dayInputs)) {
                    return;
                }
                const monthInputs = vueInputs.map(i => (<HTMLInputElement>i.$refs.monthInput).value);
                if (arrayContainsEmptyValues(monthInputs)) {
                    return;
                }
                const yearInputs = vueInputs.map(i => (<HTMLInputElement>i.$refs.yearInput).value);
                if (arrayContainsEmptyValues(yearInputs)) {
                    return;
                }
                requestData = buildKeyDateValuePairs(this.properties, dayInputs, monthInputs, yearInputs);
                this.errorFields = inputs.map((i: any) => i.$refs.errorField);
            }
            else if (inputs[0].type === "radio") {
                // TODO: Do this mapping for other types if element is reused for non-radio inputs.
                const radioInputs: Array<HTMLInputElement> = inputs;
                const inputValues = radioInputs.map(i => i.checked);
                requestData = buildKeyValuePairs(this.properties, inputValues);
                this.errorFields = this.createArrayFromRefElements("errorField");
            }

            let requestConfig = {
                method: "post" as Method,
                url: `${getValidationPath(this.$props.model)}${this.isDateValidation ? 'Dates' : 'Properties'}`,
                headers: getHeaders(),
                data: {
                    "KeyValuePairs": requestData
                }
            }

            axios.request(requestConfig)
                .then((response: any) => {
                    const errorMessages = response.data;
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

function buildKeyValuePairs(keys: Array<string>, values: Array<boolean>): KeyValuePairs {
    const keyValuePairs: KeyValuePairs = [];
    for (let i = 0; i < keys.length; i++) {
        keyValuePairs.push({ key: keys[i], value: values[i].toString() });
    }
    return keyValuePairs;
};

function buildKeyDateValuePairs(keys: Array<string>, days: Array<string>, months: Array<string>, years: Array<string>): KeyValuePairs {
    const keyValuePairs: KeyValuePairs = [];
    for (let i = 0; i < keys.length; i++) {
        keyValuePairs.push({ key: keys[i], day: days[i], month: months[i], year: years[i] });
    }
    return keyValuePairs;
};

function arrayContainsEmptyValues(array: Array<string>): boolean {
    return array.some(el => !el);
}

export default ValidateMultiple;