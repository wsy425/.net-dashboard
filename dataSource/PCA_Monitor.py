from pyod.models.pca import PCA
from joblib import dump, load


class PCA_Monitor:

    def __init__(self, n_compoents=0.95, contamination=0.02):
        self.AccumulationContribution = n_compoents
        self.Contamination = contamination
        self.Threshold = 0.0
        self.Model = None

    def ModelBuilding(self, data_train):
        clf = PCA(n_components=self.AccumulationContribution, contamination=self.Contamination)
        clf.fit(data_train)
        self.Threshold = clf.threshold_
        # self.Threshold=self.Model.threshold_
        self.Model = clf

    def SaveModel(self, path, name):
        if self.Model != None:
            fullpath = path + "/" + name + ".joblib"
            dump(self.Model, fullpath)

    def LoadModel(self, fullpath):
        self.Model = load(fullpath)

    def Predict(self, data):
        state = self.Model.predict(data)
        scores = self.Model.decision_function(data)
        return [state, scores, self.Model.threshold_]

    def CustomPredict(self, custom_threshold, data):
        scores = self.Model.decision_function(data)
        state = scores > custom_threshold
        return [state, scores, custom_threshold]
