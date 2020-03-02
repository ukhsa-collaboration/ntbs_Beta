import Vue from 'vue';

// Component to validate date order. Compares the members of the "dates" array, in ascending index order.
// Bind dates using v-model, and have bound component emit a datechanged event when date changes.
const DateComparison = Vue.extend({
    props: {
        numberOfDates: Number
    },
    data: function () {
        return {
            dates: [],
            dateWarnings: []
        }
    },
    mounted: function () {
        // wait for children to mount
        this.$nextTick(function ()
        {
            for (let i = 0; i < this.dates.length; i++) {
                this.datechanged(i);
            }
            for (let i = 0; i < this.$props.numberOfDates; i++) {
                this.$refs[`date-warning-${i}`]?.classList.remove("hidden");
            }
        })
    },
    methods: {
        datechanged: function (rank: any) {
            let i;
            const numberOfDates = this.dates.length;
            const currentIndex = parseInt(rank, 10);
            const currentDate = this.dates[currentIndex];

            // this.clearWarning(currentIndex, numberOfDates);
            
            if (!currentDate) {
                return;
            }

            if (this.$refs[`checkbox${currentIndex}`] && !this.$refs[`checkbox${currentIndex}`].checked) {
                return;
            }

            // for (i = 0; i <= numberOfDates; i++) {
            //     // We clear this so the warning is shown with respect to current date
            //     this.unsetMatchingWarning(i, currentIndex);
            // }
            this.clearWarning(currentIndex, numberOfDates);
            
            let lowerDateIndex = currentIndex - 1;
            while (lowerDateIndex >= 0) {
                if (!this.dates[lowerDateIndex] || (this.$refs[`checkbox${lowerDateIndex}`] && !this.$refs[`checkbox${lowerDateIndex}`].checked)) {
                    lowerDateIndex--;
                    continue;
                }

                if (currentDate < this.dates[lowerDateIndex]) {
                    if (this.dateWarnings[lowerDateIndex] && this.dateWarnings[lowerDateIndex].comparedTo > currentIndex)
                    {
                        continue
                    }
                    this.$set(this.dateWarnings, lowerDateIndex,
                        {
                            message: warningMessageLater(this.$refs[`date${lowerDateIndex}`].name, this.$refs[`date${currentIndex}`].name),
                            comparedTo: currentIndex
                        });
                }
                lowerDateIndex--;
            }

            let higherDateIndex = currentIndex + 1;
            // for (i = higherDateIndex; i < numberOfDates; i++) {
            //     // We clear this so the warning is shown with respect to current date
            //     this.unsetMatchingWarning(i, currentIndex);
            // }
            while (higherDateIndex < numberOfDates) {
                if (!this.dates[higherDateIndex] || (this.$refs[`checkbox${higherDateIndex}`] && !this.$refs[`checkbox${higherDateIndex}`].checked)) {
                    higherDateIndex++;
                    continue;
                }

                if (this.dates[higherDateIndex] < currentDate) {
                    this.$set(this.dateWarnings, currentIndex,
                        {
                            message: warningMessageLater(this.$refs[`date${currentIndex}`].name, this.$refs[`date${higherDateIndex}`].name),
                            comparedTo: higherDateIndex
                        });
                    return;
                }
                higherDateIndex++;
            }
            
            
        },
        clearWarning: function (currentIndex: number, numberOfDates: number) {
            if (this.dateWarnings[currentIndex]) {
                this.$set(this.dateWarnings, currentIndex, null);
            }
            for (let i = 0; i < numberOfDates; i++) {
                if (i === currentIndex) {
                    continue;
                }
                this.unsetMatchingWarning(i, currentIndex);
            }
        },
        unsetMatchingWarning: function (i: number, currentIndex: number) {
            if (this.dateWarnings[i] && this.dateWarnings[i].comparedTo === currentIndex) {
                this.$set(this.dateWarnings, i, null);
            }
        }
    },
});

function warningMessageLater(first: string, second: string) {
    return `${first} is later than ${second}`;
}

export default DateComparison;
