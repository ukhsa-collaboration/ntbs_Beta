import 'nhsuk-frontend/packages/nhsuk'
import 'nhsuk-frontend/packages/nhsuk.scss'
import '../css/site.css'
import Vue from 'vue';
import { TestButton } from './Components/TestButton';
import { ValidateInput } from './Components/ValidateInput';

// register Vue components
Vue.component('test-button', TestButton);
Vue.component('validate-input', ValidateInput);

new Vue({
  el: '#app',
});

// Enables ASP hot relaod
module.hot.accept()