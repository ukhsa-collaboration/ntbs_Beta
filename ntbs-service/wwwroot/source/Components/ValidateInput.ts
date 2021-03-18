import Vue from "vue";
import {getHeaders, getValidationPath} from "../helpers";
import axios, {Method} from "axios";

const ValidateInput = Vue.extend({
    props: ["model", "property", "shouldvalidatefull"],
    mounted: function() {
        this.$emit("mounted");
    },
    methods: {
        validate: function (event: FocusEvent) {
            // Our onBlur validate events happen on input fields
            const inputField = event.target as HTMLInputElement;
            const namesArray = inputField.name.split('.');
            
            if (this.$props.property !== namesArray[namesArray.length-1]) {
                return;
            }
            
            const newValue = inputField.value;
            this.$emit("validate", event);

            const requestConfig = {
                method: "post" as Method,
                url: `${getValidationPath(this.$props.model)}Property`,
                headers: getHeaders(),
                data: {
                    "value": newValue || null,
                    "shouldValidateFull": this.$props.shouldvalidatefull?.toLowerCase() == "true",
                    "key": this.$props.property,
                    [this.$props.property]: newValue
                }
            };
            axios.request(requestConfig)
                .then((response: any) => {
                    const errorMessage = response.data;
                    if (errorMessage) {
                        this.$emit("invalid");
                        this.$el.classList.add("nhsuk-form-group--error");
                        this.$refs["errorField"].classList.remove("hidden");
                        if (this.$refs["inputField"]) {
                            this.$refs["inputField"].classList.add("nhsuk-input--error");
                        }
                        if (this.$refs["selectField"]) {
                            this.$refs["selectField"].classList.add("nhsuk-select--error");
                        }
                        if (this.$refs["textareaField"]) {
                            this.$refs["textareaField"].classList.add("nhsuk-textarea--error");
                        }
                    } else {
                        this.$emit("valid", newValue);
                        this.$el.classList.remove("nhsuk-form-group--error");
                        this.$refs["errorField"].classList.add("hidden");
                        if (this.$refs["inputField"]) {
                            this.$refs["inputField"].classList.remove("nhsuk-input--error");
                        }
                        if (this.$refs["selectField"]) {
                            this.$refs["selectField"].classList.remove("nhsuk-select--error");
                        }
                        if (this.$refs["textareaField"]) {
                            this.$refs["textareaField"].classList.remove("nhsuk-textarea--error");
                        }
                    }
                    this.$refs["errorField"].textContent = errorMessage;
                })
                .catch((error: any) => {
                    console.log(error.response);
                });
        }
    }
});

export default ValidateInput;