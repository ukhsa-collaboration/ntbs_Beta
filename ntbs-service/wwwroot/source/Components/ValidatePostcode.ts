import Vue from "vue";
import {getHeaders, buildPath} from "../helpers";
import axios, {Method} from "axios";

const ValidatePostcode = Vue.extend({
    props: ["shouldvalidatefull"],
    methods: {
        validate: function (event: FocusEvent) {
            // Our onBlur validate events happen on input fields
            const inputField = event.target as HTMLInputElement;
            const newValue = inputField.value;

            let requestConfig = {
                method: "post" as Method,
                url: buildPath("ValidatePostcode"),
                headers: getHeaders(),
                data: {
                    "shouldValidateFull": this.$props.shouldvalidatefull.toLowerCase() == "true",
                    "postcode": newValue
                }
            }

            axios.request(requestConfig)
                .then((response: any) => {
                    const errorMessages = response.data;
                    if (errorMessages) {
                        this.$refs["formGroup"].classList.add("nhsuk-form-group--error");
                        this.$refs["inputField"].classList.add("nhsuk-input--error");
                        this.$refs["errorField"].classList.remove("hidden");
                        // Get the first error message that was returned
                        this.$refs["errorField"].textContent = errorMessages[Object.keys(errorMessages)[0]];
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

export default ValidatePostcode;