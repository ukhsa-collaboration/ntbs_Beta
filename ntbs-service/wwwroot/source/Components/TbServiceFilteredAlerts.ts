import Vue from "vue";

const TbServiceFilteredAlerts = Vue.extend({
    data() {
        return {
            TbServiceCode: "Show all"
        }
    },
    methods: {
        updateTable: function () {
            this.TbServiceCode = this.$refs["tbService"].value;
        }
    }
});

export default TbServiceFilteredAlerts;