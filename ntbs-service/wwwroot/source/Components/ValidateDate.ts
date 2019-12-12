import Vue from "vue";
import { getHeaders, getValidationPath as getValidationPath, FormattedDate, convertFormattedDateToDate } from "../helpers";
const axios = require("axios");

const ValidateDate = Vue.extend({
    props: ["model", "property", "notification_id", "name", "rank"],
    mounted: function () {
        if (this.rank) {
            // v-model binds to the input event, so this gets picked up by the containing DateComparison component, if present
            // See https://vuejs.org/v2/guide/components.html#Using-v-model-on-Components
            const formattedDate: FormattedDate = this.getFormattedDate();
            if (formattedDate) {
                this.$emit("input", convertFormattedDateToDate(formattedDate));
            }
        }
    },
    methods: {
        validate: function () {
            var date: FormattedDate = this.getFormattedDate();
            if (!date) {
                if (this.rank) {
                    this.$emit("input", null);
                    this.$parent.datechanged(this.rank);
                }
                return;
            }

            let requestConfig = {
                url: `${getValidationPath(this.$props.model)}Date`,
                headers: getHeaders(),
                params: {
                    "day": date.day,
                    "month": date.month,
                    "year": date.year,
                    "key": this.$props.property,
                    // This is an optional parameter. Pass this if the property validation depends on other properties.
                    "notificationId": this.$props.notification_id
                }
            }

            axios.request(requestConfig)
                .then((response: any) => {
                    var errorMessage = response.data;

                    this.$refs["errorField"].textContent = errorMessage;
                    if (errorMessage) {
                        this.$el.classList.add("nhsuk-form-group--error");
                        this.$refs["dayInput"].classList.add("nhsuk-input--error");
                        this.$refs["monthInput"].classList.add("nhsuk-input--error");
                        this.$refs["yearInput"].classList.add("nhsuk-input--error");
                        this.$refs["errorField"].classList.remove("hidden");
                    } else {
                        if (this.rank) {
                            this.$emit("input", convertFormattedDateToDate(date));
                            this.$parent.datechanged(this.rank);
                        }
                        this.$el.classList.remove("nhsuk-form-group--error");
                        this.$refs["dayInput"].classList.remove("nhsuk-input--error");
                        this.$refs["monthInput"].classList.remove("nhsuk-input--error");
                        this.$refs["yearInput"].classList.remove("nhsuk-input--error");
                        this.$refs["errorField"].classList.add("hidden");
                        this.$emit("validate-multiple");
                    }
                })
                .catch((error: any) => {
                    console.log(error.response);
                });
        },
        getFormattedDate: function () {
            const dayInput = this.$refs["dayInput"];
            const monthInput = this.$refs["monthInput"];
            const yearInput = this.$refs["yearInput"];

            const dayValue = dayInput.value;
            const monthValue = monthInput.value;
            const yearValue = yearInput.value;

            if (dayValue === "" || monthValue === "" || yearValue === "") {
                return null;
            }

            const formattedDate: FormattedDate = {
                day: dayValue,
                month: monthValue,
                year: yearValue
            };

            return formattedDate;
        }
    }
});

export {
    ValidateDate
};