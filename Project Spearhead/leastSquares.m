function wHat = leastSquares(z,x,order)
%Calculates the optimal weight matrix

Psi = makepolymat(x,order); % Regressor Matrix

wHat = Psi'*Psi\Psi'*z;     % Compute Weights

end