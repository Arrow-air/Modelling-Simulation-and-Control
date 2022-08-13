function [data] = spearheadCoefficients(alpha,beta,adeflection,edeflection,rdeflection,Coeff,adb,l_elevon,r_elevon,rudder)
list = {'CFX','CFY','CFZ','CMX','CMY','CMZ'};
output_selection = {Coeff};

beta = roundn(beta * 180/pi,1); % in degrees
if beta > 180
  beta = 180;
end
if beta < -180
  beta = -180;
end
alpha = roundn(alpha  * 180/pi,1); % in degrees
if alpha > 90
  alpha = 90;
end
if alpha < -90
  alpha = -90;
end

for i=1:size(adb.data,1)
  if (roundn(adb.data(i,1),1) == beta && roundn(adb.data(i,2),1) == alpha)
    for j = 1:size(list,2)
      if isequal(output_selection{1},list{j})
        datalistA = adb.data(i,j+2);
        %namelistA = list{j};
      end
    end
  end
end
% plot(adb.data(514:532,2),adb.data(514:532,3))

data = datalistA;
end
