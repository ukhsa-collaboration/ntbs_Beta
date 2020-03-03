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
            for (let i = 0; i < this.$props.numberOfDates; i++) {
                this.$refs[`date-warning-${i}`]?.classList.remove("hidden");
            }
            this.datechanged();
        })
    },
    methods: {
        datechanged: function() {
            for (let i = 0; i < this.dates.length; i++) {
                this.calculateDateWarningsRelativeToCurrentIndex(i);
            }
        },
        calculateDateWarningsRelativeToCurrentIndex: function (currentIndex: number) {
            const numberOfDates = this.dates.length;
            const currentDate = this.dates[currentIndex];
            
            if (!currentDate) {
                return;
            }

            if (this.$refs[`checkbox${currentIndex}`] && !this.$refs[`checkbox${currentIndex}`].checked) {
                return;
            }

            this.clearWarning(currentIndex, numberOfDates);
            
            let lowerDateIndex = currentIndex - 1;
            while (lowerDateIndex >= 0) {
                if (!this.dates[lowerDateIndex] || (this.$refs[`checkbox${lowerDateIndex}`] && !this.$refs[`checkbox${lowerDateIndex}`].checked)) {
                    lowerDateIndex--;
                    continue;
                }

                if (currentDate < this.dates[lowerDateIndex]) {
                    if (!(this.dateWarnings[currentIndex] && this.dateWarnings[currentIndex].comparedTo > lowerDateIndex)) {
                        this.$set(this.dateWarnings, currentIndex,
                            {
                                message: warningMessageEarlier(this.$refs[`date${currentIndex}`].name, this.$refs[`date${lowerDateIndex}`].name),
                                comparedTo: lowerDateIndex
                            });
                    }
                }
                lowerDateIndex--;
            }

            let higherDateIndex = currentIndex + 1;
            while (higherDateIndex < numberOfDates) {
                if (!this.dates[higherDateIndex] || (this.$refs[`checkbox${higherDateIndex}`] && !this.$refs[`checkbox${higherDateIndex}`].checked)) {
                    higherDateIndex++;
                    continue;
                }

                if (this.dates[higherDateIndex] < currentDate) {
                    if (!(this.dateWarnings[currentIndex] && this.dateWarnings[currentIndex].comparedTo > higherDateIndex)) {
                        this.$set(this.dateWarnings, higherDateIndex,
                            {
                                message: warningMessageEarlier(this.$refs[`date${higherDateIndex}`].name, this.$refs[`date${currentIndex}`].name),
                                comparedTo: currentIndex
                            });
                        return;
                    }
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

function warningMessageEarlier(first: string, second: string) {
    return `${first} is earlier than ${second}`;
}

export default DateComparison;
