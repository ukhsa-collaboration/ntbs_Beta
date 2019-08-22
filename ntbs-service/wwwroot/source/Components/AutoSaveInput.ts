import Vue from 'vue';
const axios =  require('axios');

const AutoSaveInput = Vue.extend({
    props: ['property'],
    data: () => {
      return {
        errorMessage: ''
      }},
  methods: {
    validate: function () {
        var headers = {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
        }

        axios.post('/Patients/Create/ValidateProperty?key='+ this.$props.property + '&value=' + this.value, null, { headers: headers })
        .then((response: any) => {
            console.log(response);
            this.errorMessage = response.data;
          })
        .catch((error: any) => {
            console.log(error.response)
        });
    }
  }
});

export {
    AutoSaveInput
};