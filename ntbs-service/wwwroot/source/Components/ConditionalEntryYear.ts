import Vue from "vue";
import { getHeaders, getValidationPath as getValidationPath } from "../helpers";
import axios from "axios";

const ConditionalEntryYear = Vue.extend({
    props: {
        domesticOrUnknownValues: {
            type: Array
        }
    },
    methods: {
        innerValidateMounted: function () {
            const value = this.getCountryIdFromChild();
            this.hideOrShowControlBasedOnValue(value);
        },
        getCountryIdFromChild: function () {
            if (this.$refs["inner-validate"] && this.$refs["inner-validate"].$refs["selectField"])
                return this.$refs["inner-validate"].$refs["selectField"].value;
            return null;
        },
        handleChange: function (event: FocusEvent) {
            const inputField = event.target as HTMLInputElement;
            const inputValue = inputField.value;
            this.hideOrShowControlBasedOnValue(inputValue);
        },
        hideOrShowControlBasedOnValue: function (value: String) {
            if (!value || this.$props.domesticOrUnknownValues.indexOf(value) !== -1) {
                this.$refs["year-of-entry-conditional"].classList.add("hidden");
            } else {
                this.$refs["year-of-entry-conditional"].classList.remove("hidden");
            }
        },
        validate: function (event: FocusEvent) {
            const inputField = event.target as HTMLInputElement;
            const inputValue = inputField.value;

            // If countryId not populated or domestic/unknown then no point continuing as
            // conditional control should not be visible.
            const countryId = this.getCountryIdFromChild();
            if (!countryId || this.$props.domesticOrUnknownValues.indexOf(countryId) !== -1)
                return;

            const requestConfig = {
                url: `${getValidationPath("YearOfUkEntry")}`,
                headers: getHeaders(),
                params: {
                    "yearOfUkEntry": inputValue
                }
            };

            axios.request(requestConfig)
                .catch((error: any) => {
                    console.log(error.response);
                })
                .then((response: any) => {
                    const errorMessage = response.data;
                    if (errorMessage) {
                        this.$refs["formGroup"].classList.add("nhsuk-form-group--error");
                        this.$refs["inputField"].classList.add("nhsuk-input--error");
                        this.$refs["errorField"].textContent = errorMessage;
                        this.$refs["errorField"].classList.remove("hidden");
                    } else {
                        this.$refs["formGroup"].classList.remove("nhsuk-form-group--error");
                        this.$refs["inputField"].classList.remove("nhsuk-input--error");
                        this.$refs["errorField"].textContent = "";
                        this.$refs["errorField"].classList.add("hidden");
                    }
                });
        }
    }
});

export default ConditionalEntryYear;