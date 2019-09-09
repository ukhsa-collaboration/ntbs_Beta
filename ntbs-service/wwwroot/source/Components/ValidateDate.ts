import Vue from 'vue';
import { getValidationBaseBath, FormattedDate, convertFormattedDateToDate } from '../helpers';
const axios = require('axios');

const ValidateDate = Vue.extend({
    props: ['model', 'property', 'name', 'rank'],
    data: function() {
        return {
            hasError: false
        }
    },
    mounted: function () {
        if (this.rank) {
            // v-model binds to the input event, so this gets picked up by the containing DateComparison component, if present
            // See https://vuejs.org/v2/guide/components.html#Using-v-model-on-Components
            var formattedDate: FormattedDate = this.getFormattedDate();
            if (formattedDate) {
                this.$emit('input', convertFormattedDateToDate(formattedDate));
            }
        }
    },
    methods: {
        validate: function () {
            var date: FormattedDate = this.getFormattedDate();
            if (!date) {
                return;
            }

            var headers = {
                "RequestVerificationToken": (<HTMLInputElement>document.querySelector('[name="__RequestVerificationToken"]')).value
            }

            axios.post(`${getValidationBaseBath(this.$props.model)}Date?key=${this.$props.property}&day=${date.day}&month=${date.month}&year=${date.year}`, 
                    null, { headers: headers })
                .then((response: any) => {
                    console.log(response);
                    var errorMessage = response.data;
                    this.hasError = errorMessage != '';
                    this.$refs["errorField"].textContent = errorMessage;
                    if (!this.hasError && this.rank) {
                        this.$emit('input', convertFormattedDateToDate(date));
                        this.$parent.datechanged(this.rank);
                    }
                })
                .catch((error: any) => {
                    console.log(error.response)
                });
        },
        getFormattedDate: function() {
            const dayInput = this.$refs["dayInput"];
            const monthInput = this.$refs["monthInput"];
            const yearInput = this.$refs["yearInput"];

            const dayValue = dayInput.value;
            const monthValue = monthInput.value;
            const yearValue = yearInput.value;

            if (dayValue === '' || monthValue === '' || yearValue === '') {
                return null;
            }

            var formattedDate: FormattedDate = {
                day: dayValue,
                month: monthValue,
                year: yearValue
            }

            return formattedDate;
        }
    }
});

export {
    ValidateDate
};