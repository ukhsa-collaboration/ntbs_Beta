import Vue from 'vue';
import { getHeaders, getValidationPath } from '../helpers';
const axios = require('axios');
const qs = require('qs');

const ValidateRequiredCheckboxes = Vue.extend({
    props: ['model', 'property', 'shouldvalidatefull'],
    methods: {
      validate: function (event: FocusEvent) {
        // Our onBlur validate events happen on input fields;;
        const inputField = event.target as HTMLInputElement
        const newValue = inputField.value;

        var checkboxes = Array.from(this.$refs["checkboxgroup"].getElementsByClassName("nhsuk-checkboxes__input"));
        var checkboxList = checkboxes
          .filter((x: any) => x.checked)
          .map((x : any) => x.id.replace("NotificationSiteMap_", "").slice(0, -1))

        var queryString = qs.stringify({
          "valueList": checkboxList,
          "shouldValidateFull": this.$props.shouldvalidatefull,
          "key": this.$props.property
        })
        let requestConfig = {
          url: `${getValidationPath(this.$props.model)}ListProperty?${queryString}`,
          headers: getHeaders(),
        }
        
        axios.request(requestConfig)
        .then((response: any) => {
            var errorMessage = response.data;
            var hasError = errorMessage != '';
            if (hasError) {
              this.$el.classList.add('nhsuk-form-group--error');
            } else {
              this.$el.classList.remove('nhsuk-form-group--error')
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
    ValidateRequiredCheckboxes
};