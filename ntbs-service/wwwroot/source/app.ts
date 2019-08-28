import { initAll as govUkJsInitAll } from 'govuk-frontend'
// Govuk css - needed for things like conditionally revealed radios sections
import 'govuk-frontend/govuk/all.scss';
import 'nhsuk-frontend/packages/nhsuk'
import 'nhsuk-frontend/packages/nhsuk.scss'
import '../css/site.css'
import Vue from 'vue';
import { TestButton } from './Components/TestButton';
import { ValidateInput } from './Components/ValidateInput';
import { ValidateDate } from './Components/ValidateDate';

// Vue needs to be the firs thing to load!
// Otherwise, it replaces the templates of its components with fresh content, potentially overwriting changes from other scripts!

// register Vue components
Vue.component('test-button', TestButton);
Vue.component('validate-input', ValidateInput);
Vue.component('validate-date', ValidateDate);

new Vue({
  el: '#app',
});

// Enables ASP hot relaod
module.hot.accept()

govUkJsInitAll()