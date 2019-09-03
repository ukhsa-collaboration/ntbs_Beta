import Vue from 'vue';
const axios = require('axios');

const CascadingDropwdowns = Vue.extend({
    methods: {
      fetchHospitalList: function (event: Event) {
          console.log("triggered")
          const selectList = event.target as HTMLInputElement
          const tbService = selectList.value;

          axios.get(`/Notifications/Edit/Episode/?handler=HospitalsByTBService&tbServiceCode=${tbService}`)
          .then((response: any) => {
            return response.data;
          })
          .then((data: any) => {
            this.$refs["hospitalDropdown"].innerHTML = "<option value=''>Select Hospital</option>";

            data.forEach((item: any) => {
              this.$refs["hospitalDropdown"].innerHTML += `<option value="${item.hospitalId}">${item.name}</option>`
            });
          })
          .catch((error: any) => {
              console.log(error.response)
          });
      }
    } 
});

export {
    CascadingDropwdowns
};