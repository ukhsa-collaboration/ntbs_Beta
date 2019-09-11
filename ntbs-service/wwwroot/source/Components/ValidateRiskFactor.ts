import Vue from 'vue';
const axios = require('axios');

type RiskFactor = { 
  inPastFiveYearsCheckbox: any, 
  moreThanFiveYearsAgoCheckbox: any,
  isCurrentCheckbox: any,
  status: any
}

const ValidateRiskFactor = Vue.extend({
  props: ['model', 'property'],
  data: function() {
    return {
      hasError: false
    }
  },
  methods: {
    validate: function (event: FocusEvent) {
        // Do nothing if focused element is part of the div that lost focus
        if (this.$el.contains(event.relatedTarget)) {
            return;
        }
        
        var headers = {
            "RequestVerificationToken": (<HTMLInputElement>document.querySelector('[name="__RequestVerificationToken"]')).value
        }

        var riskFactor = this.getRiskFactor();
        const url = `${this.$props.model}/Validate${this.$props.model}Property?key=${this.$props.property}&pastFive=${riskFactor.inPastFiveYearsCheckbox}&moreThanFive=${riskFactor.moreThanFiveYearsAgoCheckbox}&isCurrent=${riskFactor.isCurrentCheckbox}&status=${riskFactor.status}`;
        axios.post(url, null, { headers: headers })
        .then((response: any) => {
            var errorMessage = response.data;
            this.hasError = errorMessage != '';
            this.$refs["errorField"].textContent = errorMessage;
          })
        .catch((error: any) => {
            console.log(error.response)
        });
    },
    getRiskFactor: function() {
      var status = "";
      status = (this.$refs["statusYes"].checked) ? "Yes" : status;
      status = (this.$refs["statusNo"].checked) ? "No" : status;
      status = (this.$refs["statusUnknown"].checked) ? "Unknown" : status;


      var riskFactor: RiskFactor = {
        isCurrentCheckbox: this.$refs["isCurrentCheckbox"].checked,
        inPastFiveYearsCheckbox: this.$refs["inPastFiveYearsCheckbox"].checked,
        moreThanFiveYearsAgoCheckbox: this.$refs["moreThanFiveYearsAgoCheckbox"].checked,
        status: status
      }

      return riskFactor;
    }
  }
});

export {
    ValidateRiskFactor
};