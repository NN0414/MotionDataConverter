Imports Newtonsoft.Json.Linq

Public Class MotionDataConverter
    Public Event WriteLog(ByVal msg As String)

    Private Sub OnWriteLog(ByVal msg As String)
        RaiseEvent WriteLog(msg)
    End Sub

    ''' <summary>進行轉換</summary>
    ''' <param name="source">來源文件文字內容</param>
    ''' <returns></returns>
    Public Function Convert(ByVal source As String) As String
        Try
            Dim CubismFadeMotionData, SrcData As JObject
            source = ReplaceInvalidFormatString(source)
            CubismFadeMotionData = JObject.Parse(source)

            Dim Result As JObject
            Result = New JObject()
            Call WriteHead(Result)

            SrcData = CubismFadeMotionData.GetValue("0 MonoBehaviour Base")
            Call WriteCurves(SrcData, Result)

            Return Result.ToString
        Catch ex As Exception
            Call OnWriteLog("[Convert]" & ex.Message)
            Throw New Exception("請詳閱 LOG 檔" & vbCrLf & ex.ToString)
        End Try
    End Function

    ''' <summary>將無法解析的字串替換成可解析格式</summary>
    ''' <param name="source"></param>
    ''' <returns></returns>
    Private Function ReplaceInvalidFormatString(ByVal source As String) As String
        Try
            source = source.Replace("1.#INF", "Infinity")
            Return source
        Catch ex As Exception
            Call OnWriteLog("[ReplaceInvalidFormatString]" & ex.Message)
            Throw New Exception("請詳閱 LOG 檔" & vbCrLf & ex.ToString)
        End Try
    End Function

    ''' <summary>
    ''' 寫 Json 表頭
    ''' </summary>
    ''' <param name="dstData"></param>
    Private Sub WriteHead(ByRef dstData As JObject)
        Try
            'Call OnWriteLog("開始寫表頭...")

            'Meta data maybe useless for CubismFadeMotionData creations
            dstData.Add("Version", 3)
            Dim meta As JObject = New JObject()
            meta.Add("Duration", 0.0F)
            meta.Add("Fps", 0.0F)
            meta.Add("Loop", False)
            meta.Add("AreBeziersRestricted", False)
            meta.Add("CurveCount", 0.0F)
            meta.Add("TotalSegmentCount", 0.0F)
            meta.Add("TotalPointCount", 0.0F)
            meta.Add("UserDataCount", 0.0F)
            meta.Add("TotalUserDataSize", 0.0F)
            dstData.Add("Meta", meta)

            'Call OnWriteLog("表頭定義完成...")
        Catch ex As Exception
            Call OnWriteLog("[WriteHead]" & ex.Message)
            Throw New Exception("請詳閱 LOG 檔" & vbCrLf & ex.ToString)
        End Try
    End Sub

    ''' <summary>畫線 設定淡入淡出時間</summary>
    ''' <param name="srcData"></param>
    ''' <param name="dstData"></param>
    Private Sub WriteCurves(ByVal srcData As JObject, ByRef dstData As JObject)
        Try
            Dim curves, animationCurves As JArray
            Dim parameterIds As String() = GetParamaterIds(srcData)
            Dim parameterFadeInTimes As Single() = GetParameterFadeInTimes(srcData)
            Dim parameterFadeOutTimes As Single() = GetParameterFadeOutTimes(srcData)

            curves = New JArray()
            animationCurves = DirectCast(DirectCast(srcData.GetValue("0 vector ParameterCurves"), JObject).GetValue("1 Array Array"), JArray)

            'Call OnWriteLog("開始畫線...")

            '畫線迴圈
            For i = 0 To parameterIds.Length - 1
                '跳過沒填值或是空的 參數
                If String.IsNullOrEmpty(parameterIds(i)) Then
                    Continue For  '繼續迴圈 回到 for loop 那邊去
                End If
                Dim curve As JObject = New JObject()
                curve.Add("Target", "Parameter")  'Content
                curve.Add("Id", parameterIds(i))
                If parameterFadeInTimes(i) >= 0.0F Then
                    curve.Add("FadeInTime", parameterFadeInTimes(i))
                End If
                If parameterFadeOutTimes(i) >= 0.0F Then
                    curve.Add("FadeOutTime", parameterFadeOutTimes(i))
                End If
                curve.Add("Segments", ConvertKeyFramesToCurveSegments(DirectCast(animationCurves(i), JObject).GetValue("0 AnimationCurve data")))
                curves.Add(curve)
            Next
            '把憲的資料加入 dstData
            dstData.Add("Curves", curves)

            'Call OnWriteLog("畫線完成...")
        Catch ex As Exception
            Call OnWriteLog("[WriteCurves]" & ex.Message)
            Throw New Exception("[WriteCurves]" & vbCrLf & ex.ToString)
        End Try
    End Sub

    Private Function GetParamaterIds(ByVal srcData As JObject) As String()
        Try
            'Call OnWriteLog("取得參數ID...")

            Dim array As JArray = DirectCast(DirectCast(srcData.GetValue("0 vector ParameterIds"), JObject).GetValue("1 Array Array"), JArray)
            Dim result(array.Count - 1) As String
            For i = 0 To array.Count - 1
                result(i) = DirectCast(array(i), JObject).GetValue("1 string data").ToString
            Next

            'Call OnWriteLog("取得參數ID完成...")

            Return result
        Catch ex As Exception
            Call OnWriteLog("[GetParamaterIds]" & ex.Message)
            Throw New Exception("[GetParamaterIds]" & ex.ToString)
        End Try
    End Function

    Private Function GetParameterFadeInTimes(ByVal srcData As JObject) As Single()
        Try
            'Call OnWriteLog("解析淡入特效參數...")

            Dim array As JArray = DirectCast(DirectCast(srcData.GetValue("0 vector ParameterFadeInTimes"), JObject).GetValue("1 Array Array"), JArray)
            Dim result(array.Count - 1) As Single
            For i = 0 To array.Count - 1
                result(i) = CType(DirectCast(array(i), JObject).GetValue("0 float data"), Single)
            Next

            'Call OnWriteLog("解析淡入特效參數完成...")

            Return result
        Catch ex As Exception
            Call OnWriteLog("[GetParameterFadeInTimes]" & ex.Message)
            Throw New Exception("[GetParameterFadeInTimes]" & ex.ToString)
        End Try
    End Function

    Private Function GetParameterFadeOutTimes(ByVal srcData As JObject) As Single()
        Try
            'Call OnWriteLog("解析淡出特效參數完成...")

            Dim array As JArray = DirectCast(DirectCast(srcData.GetValue("0 vector ParameterFadeOutTimes"), JObject).GetValue("1 Array Array"), JArray)
            Dim result(array.Count - 1) As Single
            For i = 0 To array.Count - 1
                result(i) = CType(DirectCast(array(i), JObject).GetValue("0 float data"), Single)
            Next

            'Call OnWriteLog("解析淡出特效參數完成...")

            Return result
        Catch ex As Exception
            Call OnWriteLog("[GetParameterFadeOutTimes]" & ex.Message)
            Throw New Exception("[GetParameterFadeOutTimes]" & ex.ToString)
        End Try
    End Function

    Private Function ConvertKeyFramesToCurveSegments(ByVal animationCurves As JObject) As JArray
        Try
            'Call OnWriteLog("轉換動作片段...")

            Dim result, curveArray As JArray
            result = New JArray()
            curveArray = DirectCast(DirectCast(animationCurves.GetValue("0 vector m_Curve"), JObject).GetValue("1 Array Array"), JArray)

            If curveArray.Count > 0 Then
                Dim keyframe(curveArray.Count - 1) As Keyframe
                keyframe = ConvertJsonToArray(curveArray)
                result.Add(keyframe(0).time)
                result.Add(keyframe(0).value)

                For j = 1 To keyframe.Length - 1
                    'Judege keyframe type

                    If (j + 1 < keyframe.Length AndAlso
                        keyframe(j).inSlope <> 0.0F AndAlso
                        keyframe(j).outSlope = 0.0F AndAlso
                        keyframe(j + 1).inSlope = 0.0F AndAlso
                        keyframe(j + 1).inSlope = 0.0F) Then
                        result.Add(3.0F)  'Type: InverseStepped
                        result.Add(keyframe(j + 1).time)
                        result.Add(keyframe(j + 1).value)
                        j += 1
                    ElseIf (Single.IsPositiveInfinity(keyframe(j).inSlope)) Then
                        result.Add(2.0F)  'Type: Stepped
                        result.Add(keyframe(j).time)
                        result.Add(keyframe(j).value)
                    ElseIf (keyframe(j - 1).outSlope = keyframe(j).inSlope) Then
                        result.Add(0.0F)  'Type: Linear
                        result.Add(keyframe(j).time)
                        result.Add(keyframe(j).value)
                    Else
                        result.Add(1.0F)  'Type: Bizier
                        'Math.Abs() 取絕對值
                        Dim tangentLength As Single = Math.Abs(keyframe(j - 1).time - keyframe(j).time) * 0.333333F
                        result.Add(0.0F)
                        result.Add(keyframe(j - 1).outSlope * tangentLength + keyframe(j - 1).value)
                        result.Add(0.0F)
                        result.Add(keyframe(j).value - keyframe(j).inSlope * tangentLength)
                        result.Add(keyframe(j).time)
                        result.Add(keyframe(j).value)
                    End If
                Next
            End If

            'Call OnWriteLog("轉換動作片段完成...")

            Return result
        Catch ex As Exception
            Call OnWriteLog("[ConvertKeyFramesToCurveSegments]" & ex.Message)
            Throw New Exception("[ConvertKeyFramesToCurveSegments]" & ex.ToString)
        End Try
    End Function

    Private Function ConvertJsonToArray(ByVal array As JArray) As Keyframe()
        Try
            'Call OnWriteLog("定義碰撞箱參數...")

            Dim result(array.Count - 1) As Keyframe
            For i = 0 To array.Count - 1
                Dim obj As JObject = DirectCast(DirectCast(array(i), JObject).GetValue("0 Keyframe data"), JObject)
                result(i) = New Keyframe
                result(i).time = CType(obj.GetValue("0 float time"), Single)
                result(i).value = CType(obj.GetValue("0 float value"), Single)
                result(i).inSlope = CType(obj.GetValue("0 float inSlope"), Single)
                result(i).outSlope = CType(obj.GetValue("0 float outSlope"), Single)
            Next
            Return result

            'Call OnWriteLog("定義碰撞箱參數完成...")
        Catch ex As Exception
            Call OnWriteLog("[ConvertJsonToArray]" & ex.Message)
            Throw New Exception("[ConvertJsonToArray]" & ex.ToString)
        End Try
    End Function
End Class

Friend Class Keyframe
    Public time, value, inSlope, outSlope As Single
End Class