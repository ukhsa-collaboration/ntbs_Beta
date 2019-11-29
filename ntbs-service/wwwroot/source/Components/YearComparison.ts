import Vue from "vue";
import { getHeaders, getValidationPath } from "../helpers";
import axios from "axios";

const YearComparison = Vue.extend({
    props: ["model", "yeartocompare", "shouldvalidatefull", "propertyname"],
    methods: {
        validate: function (event: FocusEvent) {
            // For validating an input year against a year on a different model, which needs to be passed in as the yeartocompare prop
            const inputField = event.target as HTMLInputElement;
            const newValue = inputField.value;

            const requestConfig = {
                url: `${getValidationPath(this.$props.model)}YearComparison`,
                headers: getHeaders(),
                params: {
                    "newYear": newValue,
                    "shouldValidateFull": this.$props.shouldvalidatefull,
                    "existingYear": this.$props.yeartocompare,
                    "propertyName": this.$props.propertyname
                }
            };
            axios.request(requestConfig)
                .then((response: any) => {
                    var errorMessage = response.data;
                    if (errorMessage) {
                        this.$el.classList.add("nhsuk-form-group--error");
                        this.$refs["errorField"].textContent = errorMessage;
                        this.$refs["errorField"].classList.remove("hidden");
                        if (this.$refs["inputField"]) {
                            this.$refs["inputField"].classList.add("nhsuk-input--error");
                        }
                    } else {
                        this.$el.classList.remove("nhsuk-form-group--error");
                        this.$refs["errorField"].textContent = "";
                        this.$refs["errorField"].classList.add("hidden");
                        if (this.$refs["inputField"]) {
                            this.$refs["inputField"].classList.remove("nhsuk-input--error");
                        }
                    }
                })
                .catch((error: any) => {
                    console.log(error.response);
                });
        }
    }
});

export {
    YearComparison
};