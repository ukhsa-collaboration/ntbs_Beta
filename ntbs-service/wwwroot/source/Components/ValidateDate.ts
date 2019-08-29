import Vue from 'vue';
const axios = require('axios');

const ValidateDate = Vue.extend({
    props: ['model', 'property'],
    data: function() {
      return {
        hasError: false,
      }},
  methods: {
    validate: function () {
        // Our onBlur validate events happen on input fields
        const dayInput = this.$refs["dayInput"];
        const monthInput = this.$refs["monthInput"];
        const yearInput = this.$refs["yearInput"];

        const dayValue = dayInput.value;
        const monthValue = monthInput.value;
        const yearValue = yearInput.value;

        if (dayValue === '' || monthValue === '' || yearValue === '') {
            return;
        }

        // TODO - remove jquery and use a vue-way of getting this value.
        var headers = {
            "RequestVerificationToken": (<HTMLInputElement>document.querySelector('[name="__RequestVerificationToken"]')).value
        }

        axios.post(`/${this.$props.model}s/Edit/Validate${this.$props.model}Date?key=${this.$props.property}&day=${dayValue}&month=${monthValue}&year=${yearValue}`, 
                null, { headers: headers })
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
    ValidateDate
};


// for validation between dates:
// add a hidden input field that generates date from the three input fields (to make it simpler)
// when validation passes, emit an event to the containing datecomparison component
// this component then can fetch all the hidden input dates (how will it have access?) and compare them