import Vue from 'vue';
import { getHeaders, getValidationPath } from '../helpers';
const axios = require('axios');

const ValidateInput = Vue.extend({
    props: ['model', 'property', 'shouldvalidatefull'],
    methods: {
      validate: function (event: FocusEvent) {
          // Our onBlur validate events happen on input fields
          const inputField = event.target as HTMLInputElement
          const newValue = inputField.value;

        let requestConfig = {
          url: `${getValidationPath(this.$props.model)}Property`,
          headers: getHeaders(),
          params: {
            "value": newValue,
            "shouldValidateFull": this.$props.shouldvalidatefull,
            "key": this.$props.property
          }
        }
        axios.request(requestConfig)
        .then((response: any) => {
            var errorMessage = response.data;
            var hasError = errorMessage != '';
            if (hasError) {
              this.$el.classList.add('nhsuk-form-group--error');
              if (this.$refs["inputField"]) {
                this.$refs["inputField"].classList.add('nhsuk-input--error')
              }
              if (this.$refs["selectField"]) {
                this.$refs["selectField"].classList.add('nhsuk-select--error')
              }
            } else {
              this.$el.classList.remove('nhsuk-form-group--error')
              if (this.$refs["inputField"]) {
                this.$refs["inputField"].classList.remove('nhsuk-input--error')
              }
              if (this.$refs["selectField"]) {
                this.$refs["selectField"].classList.remove('nhsuk-select--error')
              }
            }
            this.$refs["errorField"].textContent = errorMessage;
          })
        .catch((error: any) => {
            console.log(error.response)
        });
    }
  }
});

export {
    ValidateInput
};