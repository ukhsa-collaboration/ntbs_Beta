import Vue from "vue";
import { getHeaders } from "../helpers";
import axios from "axios";

const ValidatePostcode = Vue.extend({
    props: ["shouldvalidatefull"],
    methods: {
        validate: function (event: FocusEvent) {
            // Our onBlur validate events happen on input fields
            const inputField = event.target as HTMLInputElement;
            const newValue = inputField.value;

            let requestConfig = {
                url: `${window.location.pathname}/ValidatePostcode`,
                headers: getHeaders(),
                params: {
                    "shouldValidateFull": this.$props.shouldvalidatefull || false,
                    "postcode": newValue
                }
            }

            axios.request(requestConfig)
                .then((response: any) => {
                    var errorMessages = response.data;
                    if (errorMessages) {
                        this.$refs["formGroup"].classList.add("nhsuk-form-group--error");
                        this.$refs["inputField"].classList.add("nhsuk-input--error");
                        this.$refs["errorField"].classList.remove("hidden");
                        this.$refs["errorField"].textContent = errorMessages[0];
                    } else {
                        this.$refs["formGroup"].classList.remove("nhsuk-form-group--error");
                        this.$refs["inputField"].classList.remove("nhsuk-input--error");
                        this.$refs["errorField"].classList.add("hidden");
                        this.$refs["errorField"].textContent = "";
                    }
                })
                .catch((error: any) => {
                    console.log(error.response);
                });
        }
    }
});

export {
    ValidatePostcode
};