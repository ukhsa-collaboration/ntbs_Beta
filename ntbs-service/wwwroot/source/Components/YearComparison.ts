import Vue from 'vue';
import { getHeaders, getValidationPath } from '../helpers';
const axios = require('axios');

const YearComparison = Vue.extend({
  props: ['model', 'yeartocompare'],
  methods: {
    validate: function (event: FocusEvent) {
        // For validating an input year against a year on a different model, which needs to be passed in as the yeartocompare prop
        if (!this.$props.yeartocompare) {
            return;
        }
        const inputField = event.target as HTMLInputElement
        const newValue = inputField.value;

        axios.get(`${getValidationPath(this.$props.model)}YearComparison?newYear=${newValue}&existingYear=${this.$props.yeartocompare}`, null, { headers: getHeaders() })
        .then((response: any) => {
            console.log(response);
            var errorMessage = response.data;
            var hasError = errorMessage != '';
            this.$refs["errorField"].textContent = errorMessage;
            if (hasError) {
              this.$el.classList.add('nhsuk-form-group--error');
              if (this.$refs["inputField"]) {
                this.$refs["inputField"].classList.add('nhsuk-input--error')
              }
            } else {
              this.$el.classList.remove('nhsuk-form-group--error')
              if (this.$refs["inputField"]) {
                this.$refs["inputField"].classList.remove('nhsuk-input--error')
              }
            }
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