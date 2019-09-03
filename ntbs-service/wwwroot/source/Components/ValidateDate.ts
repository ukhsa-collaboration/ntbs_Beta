import Vue from 'vue';
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
            this.$emit('input', this.getDate());
        }
    },
    methods: {
        validate: function () {
            var date = this.getDate();
            if (!date) {
                return;
            }

            var headers = {
                "RequestVerificationToken": (<HTMLInputElement>document.querySelector('[name="__RequestVerificationToken"]')).value
            }

            axios.post(`/${this.$props.model}s/Edit/Validate${this.$props.model}Date?key=${this.$props.property}&day=${date.getDate()}&month=${date.getMonth() + 1}&year=${date.getFullYear()}`, 
                    null, { headers: headers })
                .then((response: any) => {
                    console.log(response);
                    var errorMessage = response.data;
                    this.hasError = errorMessage != '';
                    this.$refs["errorField"].textContent = errorMessage;
                    if (!this.hasError && this.rank) {
                        this.$emit('input', date);
                        this.$parent.datechanged(this.rank);
                    }
                })
                .catch((error: any) => {
                    console.log(error.response)
                });
        },
        getDate: function() {
            const dayInput = this.$refs["dayInput"];
            const monthInput = this.$refs["monthInput"];
            const yearInput = this.$refs["yearInput"];

            const dayValue = dayInput.value;
            const monthValue = monthInput.value;
            const yearValue = yearInput.value;

            if (dayValue === '' || monthValue === '' || yearValue === '') {
                return null;
            }

            return new Date(yearValue, monthValue - 1, dayValue);
        }
    }
});

export {
    ValidateDate
};