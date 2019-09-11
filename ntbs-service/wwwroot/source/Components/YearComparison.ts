import Vue from 'vue';
import { getHeaders } from '../helpers';
const axios = require('axios');

const YearComparison = Vue.extend({
    props: ['yeartocompare'],
    data: function() {
      return {
        hasError: false
      }},
  methods: {
    validate: function (event: FocusEvent) {
        // For validating an input year against a year on a different model, which needs to be passed in as the yeartocompare prop
        if (!this.$props.yeartocompare) {
            return;
        }
        const inputField = event.target as HTMLInputElement
        const newValue = inputField.value;

        axios.get(`Edit/ValidateYearComparison?newYear=${newValue}&existingYear=${this.$props.yeartocompare}`, null, { headers: getHeaders() })
        .then((response: any) => {
            console.log(response);
            var errorMessage = response.data;
            this.hasError = errorMessage != '';
            this.$refs["errorField"].textContent = errorMessage;
          })
        .catch((error: any) => {
            console.log(error.response)
        });
    }
  }
});

export {
    YearComparison
};