import { initAll as govUkJsInitAll } from 'govuk-frontend'
// Govuk css - needed for things like conditionally revealed radios sections
import "../css/reset.css"
import 'govuk-frontend/govuk/all.scss';
import 'nhsuk-frontend/packages/nhsuk'
import 'nhsuk-frontend/packages/nhsuk.scss'
import '../css/site.css'
import Vue from 'vue';
import { TestButton } from './Components/TestButton';
import { ValidateInput } from './Components/ValidateInput';
import { ValidateDate } from './Components/ValidateDate';
import { DateComparison } from './Components/DateComparison';
import { YearComparison } from './Components/YearComparison';
// @ts-ignore
import config from './config/config-APP_TARGET';
import { CascadingDropwdowns } from './Components/CascadingDropdowns';
import { ValidateContactTracing } from './Components/ValidateContactTracing';
import { ValidateRiskFactor } from './Components/ValidateRiskFactor';
// Vue needs to be the firs thing to load!
// Otherwise, it replaces the templates of its components with fresh content, potentially overwriting changes from other scripts!

// register Vue components
Vue.component('test-button', TestButton);
Vue.component('validate-input', ValidateInput);
Vue.component('validate-date', ValidateDate);
Vue.component('date-comparison', DateComparison);
Vue.component('cascading-dropdowns', CascadingDropwdowns);
Vue.component('validate-contact-tracing', ValidateContactTracing);
Vue.component('validate-riskfactor', ValidateRiskFactor);
Vue.component('year-comparison', YearComparison);

new Vue({
  el: '#app',
});

if (config.env === 'development') {
  // Enables ASP hot relaod
  console.log("RUNNING IN DEVELOPMENT MODE - Accepting hot reload")
  module.hot.accept()
}

govUkJsInitAll()