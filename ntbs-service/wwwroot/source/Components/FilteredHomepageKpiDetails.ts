import Vue from "vue";

const FilteredHomepageKpiDetails = Vue.extend({
    data() {
        return {
            CodeFilter: ""
        }
    },
    mounted : function () {
        this.CodeFilter = this.$refs["filterByCode"].value;
    },
    methods: {
        updateKpiDetails: function () {
            this.CodeFilter = this.$refs["filterByCode"].value;
        }
    }
});


export default FilteredHomepageKpiDetails;