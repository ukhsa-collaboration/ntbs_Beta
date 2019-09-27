import Vue from 'vue';
import { getHeaders, getValidationPath } from '../helpers';

const axios = require('axios');

const CascadingDropwdowns = Vue.extend({
    props: ['model', 'shouldvalidatefull'],
    methods: {
      fetchHospitalList: function (event: Event) {
          const selectList = event.target as HTMLInputElement
          const tbService = selectList.value;

          axios.get(`/Notifications/Edit/Episode/?handler=HospitalsByTBService&tbServiceCode=${tbService}`)
          .then((response: any) => {
            return response.data;
          })
          .then((data: any) => {
            this.$refs["selectFieldHospitalId"].innerHTML = "<option value=''>Select Hospital</option>";

            data.forEach((item: any) => {
              this.$refs["selectFieldHospitalId"].innerHTML += `<option value="${item.hospitalId}">${item.name}</option>`
            });
          })
          .catch((error: any) => {
              console.log(error.response)
          });
      },
      validate: function(property : string) {
          var shouldValidateFull = this.$props.shouldvalidatefull;
          var modelName = this.$props.model;
          // TODO: We need to check whether arrow ('=>') works in IE
          return (event: Event) => {
            // Our onBlur validate events happen on input fields
            const inputField = event.target as HTMLInputElement
            const newValue = inputField.value;
            
            let requestConfig = {
              url: `${getValidationPath(modelName)}Property`,
              headers: getHeaders(),
              params: {
                "value": newValue,
                "shouldValidateFull": shouldValidateFull,
                "key": property
              }
            }

            axios.request(requestConfig)
            .then((response: any) => {
                var errorMessage = response.data;
                var hasError = errorMessage != '';
                this.$refs["errorField" + property].textContent = errorMessage;

                if (hasError) {
                  this.$refs["formGroup" + property].classList.add('nhsuk-form-group--error');
                  this.$refs["selectField" + property].classList.add('nhsuk-select--error');  
                } else {
                  this.$refs["formGroup" + property].classList.remove('nhsuk-form-group--error');
                  this.$refs["selectField" + property].classList.remove('nhsuk-select--error');         
                }
              })
            .catch((error: any) => {
                console.log(error.response)
            });
          } 
      }
    } 
});

export {
    CascadingDropwdowns
};