using System.Collections.Generic;
using System.Linq;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Models.SeedData
{
    public class ManualTestTypeSampleTypes
    {
        public static IEnumerable<ManualTestTypeSampleType> GetJoinDataManualTestTypeToSampleType()
        {
            var smearSampleTypes = new[]
            {
                (int)SampleTypeId.BronchialWashings,
                (int)SampleTypeId.BronchoscopySample,
                (int)SampleTypeId.LungBronchialTreeTissue,
                (int)SampleTypeId.SputumInduced,
                (int)SampleTypeId.SputumSpontaneous,
                (int)SampleTypeId.Blood,
                (int)SampleTypeId.BoneAndJoint,
                (int)SampleTypeId.Csf,
                (int)SampleTypeId.Faeces,
                (int)SampleTypeId.GastricWashings,
                (int)SampleTypeId.Gynaecological,
                (int)SampleTypeId.LymphNode,
                (int)SampleTypeId.PeritonealFluid,
                (int)SampleTypeId.PleuralFluidOrBiopsy,
                (int)SampleTypeId.Pus,
                (int)SampleTypeId.Urine,
                (int)SampleTypeId.OtherTissues,
                (int)SampleTypeId.NotKnown,
            };

            var cultureSampleTypes = new[]
            {
                (int)SampleTypeId.BronchialWashings,
                (int)SampleTypeId.BronchoscopySample,
                (int)SampleTypeId.LungBronchialTreeTissue,
                (int)SampleTypeId.SputumInduced,
                (int)SampleTypeId.SputumSpontaneous,
                (int)SampleTypeId.Blood,
                (int)SampleTypeId.BoneAndJoint,
                (int)SampleTypeId.Csf,
                (int)SampleTypeId.Faeces,
                (int)SampleTypeId.GastricWashings,
                (int)SampleTypeId.Gynaecological,
                (int)SampleTypeId.LymphNode,
                (int)SampleTypeId.PeritonealFluid,
                (int)SampleTypeId.PleuralFluidOrBiopsy,
                (int)SampleTypeId.Pus,
                (int)SampleTypeId.Urine,
                (int)SampleTypeId.OtherTissues,
                (int)SampleTypeId.NotKnown,
            };

            var histologySampleTypes = new[]
            {
                (int)SampleTypeId.BronchoscopySample,
                (int)SampleTypeId.LungBronchialTreeTissue,
                (int)SampleTypeId.BoneAndJoint,
                (int)SampleTypeId.Cns,
                (int)SampleTypeId.Gastrointestinal,
                (int)SampleTypeId.Genitourinary,
                (int)SampleTypeId.LymphNode,
                (int)SampleTypeId.Pleural,
                (int)SampleTypeId.Skin,
                (int)SampleTypeId.OtherTissues,
                (int)SampleTypeId.NotKnown,
            };

            var chestXRaySampleTypes = new int[] { };

            var pcrSampleTypes = new[]
            {
                (int)SampleTypeId.BronchialWashings,
                (int)SampleTypeId.BronchoscopySample,
                (int)SampleTypeId.LungBronchialTreeTissue,
                (int)SampleTypeId.SputumInduced,
                (int)SampleTypeId.SputumSpontaneous,
                (int)SampleTypeId.Blood,
                (int)SampleTypeId.BoneAndJoint,
                (int)SampleTypeId.Csf,
                (int)SampleTypeId.Faeces,
                (int)SampleTypeId.GastricWashings,
                (int)SampleTypeId.Gynaecological,
                (int)SampleTypeId.LymphNode,
                (int)SampleTypeId.PeritonealFluid,
                (int)SampleTypeId.PleuralFluidOrBiopsy,
                (int)SampleTypeId.Pus,
                (int)SampleTypeId.Urine,
                (int)SampleTypeId.OtherTissues,
                (int)SampleTypeId.NotKnown,
            };

            var lineProbeAssaySampleTypes = new[]
            {
                (int)SampleTypeId.BronchialWashings,
                (int)SampleTypeId.BronchoscopySample,
                (int)SampleTypeId.LungBronchialTreeTissue,
                (int)SampleTypeId.SputumInduced,
                (int)SampleTypeId.SputumSpontaneous,
                (int)SampleTypeId.Blood,
                (int)SampleTypeId.BoneAndJoint,
                (int)SampleTypeId.Csf,
                (int)SampleTypeId.Faeces,
                (int)SampleTypeId.GastricWashings,
                (int)SampleTypeId.Gynaecological,
                (int)SampleTypeId.LymphNode,
                (int)SampleTypeId.PeritonealFluid,
                (int)SampleTypeId.PleuralFluidOrBiopsy,
                (int)SampleTypeId.Pus,
                (int)SampleTypeId.Urine,
                (int)SampleTypeId.OtherTissues,
                (int)SampleTypeId.NotKnown,
            };

            var joinData = new List<ManualTestTypeSampleType>();
            joinData.AddRange(smearSampleTypes.Select(sampleType => new ManualTestTypeSampleType { ManualTestTypeId = (int)ManualTestTypeId.Smear, SampleTypeId = sampleType }));
            joinData.AddRange(cultureSampleTypes.Select(sampleType => new ManualTestTypeSampleType { ManualTestTypeId = (int)ManualTestTypeId.Culture, SampleTypeId = sampleType }));
            joinData.AddRange(histologySampleTypes.Select(sampleType => new ManualTestTypeSampleType { ManualTestTypeId = (int)ManualTestTypeId.Histology, SampleTypeId = sampleType }));
            joinData.AddRange(chestXRaySampleTypes.Select(sampleType => new ManualTestTypeSampleType { ManualTestTypeId = (int)ManualTestTypeId.ChestXRay, SampleTypeId = sampleType }));
            joinData.AddRange(pcrSampleTypes.Select(sampleType => new ManualTestTypeSampleType { ManualTestTypeId = (int)ManualTestTypeId.Pcr, SampleTypeId = sampleType }));
            joinData.AddRange(lineProbeAssaySampleTypes.Select(sampleType => new ManualTestTypeSampleType { ManualTestTypeId = (int)ManualTestTypeId.LineProbeAssay, SampleTypeId = sampleType }));

            return joinData;
        }
    }
}
