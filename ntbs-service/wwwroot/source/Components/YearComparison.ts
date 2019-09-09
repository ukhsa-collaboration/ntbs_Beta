import Vue from 'vue';
import { getValidationBaseBath } from '../helpers';
const axios = require('axios');

const YearComparison = Vue.extend({
    props: ['yeartocompare'],
    data: function() {
      return {
        hasError: false
      }},
  methods: {
    validate: function (event: FocusEvent) {
        if (!this.$props.yeartocompare) {
            return;
        }
        const inputField = event.target as HTMLInputElement
        const newValue = inputField.value;
        var headers = {
            "RequestVerificationToken": (<HTMLInputElement>document.querySelector('[name="__RequestVerificationToken"]')).value
        }

        axios.post(`${getValidationBaseBath('YearComparison')}?newYear=${newValue}&existingYear=${this.$props.yeartocompare}`, null, { headers: headers })
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