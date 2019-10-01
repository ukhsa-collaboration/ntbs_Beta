import Vue from 'vue';
import { getHeaders, getValidationPath } from '../helpers';
import axios from 'axios';
import { InputType } from 'zlib';

const qs = require('qs');

const ValidateRequiredCheckboxes = Vue.extend({
    props: ['property', 'shouldvalidatefull'],
    methods: {
      validate: function (event: FocusEvent) {
        // Our onBlur validate events happen on input fields;;
        const inputField = event.target as HTMLInputElement
        const newValue = inputField.value;

        var notificationSiteRegex = /NotificationSiteMap\[(.*)\]/;
        var checkboxes : Array<HTMLInputElement> = Array.from(this.$refs["checkboxgroup"].getElementsByClassName("nhsuk-checkboxes__input"));
        var checkboxList = checkboxes
          .filter(x => x.checked)
          .map(x => notificationSiteRegex.exec(x.name)[1])

        var queryString = qs.stringify({
          "valueList": checkboxList,
          "shouldValidateFull": this.$props.shouldvalidatefull,
        })

        let requestConfig = {
          url: `${getValidationPath(this.$props.property)}?${queryString}`,
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