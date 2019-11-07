import Vue from "vue";
import { getHeaders, getValidationPath } from "../helpers";
import axios from "axios";

const qs = require("qs");

const ValidateMultiple = Vue.extend({
    props: {
        model: String,
        properties: Array
    },
    data: function () {
        return {
            hasError: false
        }
    },
    methods: {
        // Annotate values to compare with ref="input[i]", and error fields with ref="errorField[i]", for any number of inputs that should be validated together
        // Each error message will be mapped to the corresponding field.
        validate: function () {
            const inputs = this.createArrayFromRefElements("input");
            var inputValues: any;
            if (inputs[0].type === "radio") {
                // TODO: Do this mapping for other types if element is reused for non-radio inputs.
                inputValues = inputs.map((i: any) => i.checked);
            }

            const queryString = buildKeyValuePairsQueryString(this.properties, inputValues);
            axios.get(`${getValidationPath(this.$props.model)}Properties?${queryString}`, { headers: getHeaders() })
                .then((response: any) => {
                    var errorMessages = response.data;
                    this.hasError = !!errorMessages;
                    var errorFields = this.createArrayFromRefElements("errorField");

                    if (this.hasError) {
                        errorFields.forEach((errorField: any, index: number) => {
                            errorField.textContent = errorMessages[index];
                            errorField.classList.remove("hidden");
                        });
                    } else {
                        errorFields.forEach((errorField: any, index: number) => {
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

function buildKeyValuePairsQueryString(keys: Array<string>, values: Array<string>) {
    const queryStringObject: any = { keyValuePairs: [] };
    for (let i = 0; i < keys.length; i++) {
        queryStringObject["keyValuePairs"].push({ key: keys[i], value: values[i] });
    }
    return qs.stringify(queryStringObject);
};

export {
    ValidateMultiple
};