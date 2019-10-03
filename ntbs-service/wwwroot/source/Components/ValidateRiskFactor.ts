import Vue from 'vue';
import axios from 'axios';
import { getHeaders, getValidationPath } from '../helpers';

type RiskFactor = { 
  inPastFiveYearsCheckbox: boolean, 
  moreThanFiveYearsAgoCheckbox: boolean,
  isCurrentCheckbox: boolean,
  status: string
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

        var riskFactor = this.getRiskFactor();

        let requestConfig = {
          url: `${this.$props.model}/Validate${this.$props.model}Property`,
          headers: getHeaders(),
          params: {
            "pastFive": riskFactor.inPastFiveYearsCheckbox,
            "moreThanFive": riskFactor.moreThanFiveYearsAgoCheckbox,
            "isCurrent": riskFactor.isCurrentCheckbox,
            "status": riskFactor.status,
            "key": this.$props.property
          }
        }

        axios.request(requestConfig)
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