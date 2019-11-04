// Root styles import - other global styles are imported from this sass file
import "../css/site.scss"
// @ts-ignore
import config from "./config/config-APP_TARGET";
import Vue from "vue";
import { initAll as govUkJsInitAll } from "govuk-frontend";
import { ValidateInput } from "./Components/ValidateInput";
import { ValidateDate } from "./Components/ValidateDate";
import { DateComparison } from "./Components/DateComparison";
import { YearComparison } from "./Components/YearComparison";
import { CascadingDropdowns } from "./Components/CascadingDropdowns";
import { ValidateContactTracing } from "./Components/ValidateContactTracing";
import ValidateImmunosuppression from "./Components/ValidateImmunosuppression";
import ValidateTravelOrVisit from "./Components/ValidateTravelOrVisit";
import { ValidateMultiple } from "./Components/ValidateMultiple";
import { ValidateRequiredCheckboxes } from "./Components/ValidateRequiredCheckboxes";
import { ValidatePostcode } from "./Components/ValidatePostcode";
import ConditionalSelectWrapper from "./Components/ConditionalSelectWrapper";

// Vue needs to be the first thing to load!
// Otherwise, it replaces the templates of its components with fresh content, potentially overwriting changes from other scripts!

// register Vue components
Vue.component("validate-input", ValidateInput);
Vue.component("validate-date", ValidateDate);
Vue.component("date-comparison", DateComparison);
Vue.component("cascading-dropdowns", CascadingDropdowns);
Vue.component("validate-contact-tracing", ValidateContactTracing);
Vue.component("validate-immunosuppression", ValidateImmunosuppression);
Vue.component("validate-travel-or-visit", ValidateTravelOrVisit);
Vue.component("year-comparison", YearComparison);
Vue.component("validate-multiple", ValidateMultiple);
Vue.component("validate-required-checkboxes", ValidateRequiredCheckboxes);
Vue.component("validate-postcode", ValidatePostcode);
Vue.component("conditional-select-wrapper", ConditionalSelectWrapper);

new Vue({
    el: "#app",
});

if (config.env === "development") {
    // Enables ASP hot relaod
    console.log("RUNNING IN DEVELOPMENT MODE - Accepting hot reload");
    module.hot.accept();
}

govUkJsInitAll();

// For compatibility with IE11
require("es6-promise").polyfill();