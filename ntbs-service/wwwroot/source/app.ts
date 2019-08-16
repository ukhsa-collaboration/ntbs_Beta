import 'nhsuk-frontend/packages/nhsuk'
import 'nhsuk-frontend/packages/nhsuk.scss'
import '../css/site.css'
import Vue from 'vue';

const TestButton = Vue.extend({
  data: () => ({ text: 'Notify' }),
  mounted: function() {
      this.$el.innerText = this.text;
  },
  methods: {
    onBlur: function() {
      this.$el.innerText = "Bye!"
    }
  }
})

// register Vue components
Vue.component('test-button', TestButton);


new Vue({
  el: '#app',
});

// Enables ASP hot relaod
module.hot.accept()